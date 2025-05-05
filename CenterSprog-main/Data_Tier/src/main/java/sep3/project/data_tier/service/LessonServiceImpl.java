package sep3.project.data_tier.service;

import io.grpc.Status;
import io.grpc.StatusRuntimeException;
import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import org.hibernate.Hibernate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.transaction.annotation.Transactional;

import sep3.project.data_tier.entity.ClassEntity;
import sep3.project.data_tier.entity.LessonEntity;
import sep3.project.data_tier.entity.UserEntity;
import sep3.project.data_tier.mappers.HomeworkMapper;
import sep3.project.data_tier.repository.IClassRepository;
import sep3.project.data_tier.repository.ILessonRepository;
import sep3.project.data_tier.repository.IUserRepository;
import sep3.project.protobuf.*;

import java.util.*;
import java.util.stream.Collectors;

/**
 * Service class that handles the grpc communication.
 * 
 * 
 * @author Team_3
 * @version 1.0
 */
@GrpcService
public class LessonServiceImpl
    extends LessonServiceGrpc.LessonServiceImplBase {

  private ILessonRepository lessonRepository;
  private IUserRepository userRepository;
  private IClassRepository classRepository;
  private HomeworkMapper homeworkMapper = HomeworkMapper.INSTANCE;

  /**
   * 3-argument constructor for LessonServiceImpl
   * 
   * @param userRepository   - repository for user
   * @param lessonRepository - repository for lesson
   * @param classRepository  - repository for class
   */
  @Autowired
  public LessonServiceImpl(IUserRepository userRepository,
      ILessonRepository lessonRepository, IClassRepository classRepository) {
    this.userRepository = userRepository;
    this.lessonRepository = lessonRepository;
    this.classRepository = classRepository;
  }

  /**
   * Method that gets all lessons from the database
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  @Transactional
  public void getLessonById(
      RequestGetLessonById request,
      StreamObserver<ResponseGetLessonById> response) {
    try {
      String id = request.getLessonId();
      Optional<LessonEntity> existingLesson = lessonRepository.findById(id);
      if (existingLesson.isEmpty())
        throw new NoSuchElementException("No existing class with id " + id);

      Hibernate.initialize(existingLesson.get());

      LessonData grpcLesson = LessonData.newBuilder()
          .setId(existingLesson.get().getId())
          .setDate(existingLesson.get().getDate())
          .setDescription(existingLesson.get().getDescription())
          .setTopic(existingLesson.get().getTopic()).buildPartial();

      if (existingLesson.get().getHomework() != null)
        grpcLesson = grpcLesson.toBuilder().setHomework(
            homeworkMapper.toProto(existingLesson.get().getHomework()))
            .buildPartial();

      grpcLesson.toBuilder().build();

      response.onNext(
          ResponseGetLessonById.newBuilder().setLesson(grpcLesson).build());
      response.onCompleted();

    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that gets all lessons from the database
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  @Transactional
  public void getAttendance(
      RequestGetAttendance request,
      StreamObserver<ResponseGetAttendance> response) {
    try {
      String id = request.getLessonId();
      Optional<LessonEntity> existingLesson = lessonRepository.findById(id);
      if (existingLesson.isEmpty())
        throw new NoSuchElementException("No existing class with id " + id);

      Hibernate.initialize(existingLesson.get());

      List<UserParticipant> grpcUsers = new ArrayList<>();
      if (!existingLesson.get().getAttendance().isEmpty())
        for (UserEntity userEntity : existingLesson.get().getAttendance()) {
          UserParticipant grpcUser = UserParticipant.newBuilder()
              .setUsername(userEntity.getUsername())
              .setFirstName(userEntity.getFirstName())
              .setLastName(userEntity.getLastName()).build();
          grpcUsers.add(grpcUser);
        }

      response.onNext(
          ResponseGetAttendance.newBuilder().addAllParticipants(grpcUsers)
              .build());
      response.onCompleted();
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }

  }

  /**
   * Method that gets all lessons from the database
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  public void markAttendance(RequestMarkAttendance request,
      StreamObserver<ResponseMarkAttendance> response) {
    String id = request.getLessonId();
    try {
      Optional<LessonEntity> existingLesson = lessonRepository.findById(id);

      if (existingLesson.isEmpty())
        throw new NoSuchElementException("No existing lesson with id " + id);

      List<String> studentUsernames = request.getUsernamesList();
      Set<UserEntity> students = userRepository.findAll().stream()
          .filter(user -> studentUsernames.contains(user.getUsername()))
          .collect(Collectors.toSet());

      existingLesson.get().setAttendance(students);
      lessonRepository.save(existingLesson.get());

      response.onNext(ResponseMarkAttendance.newBuilder()
          .setAmountOfParticipants(students.size()).build());
      response.onCompleted();
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that gets all lessons from the database
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  @Transactional
  public void deleteLesson(RequestDeleteLesson request,
      StreamObserver<ResponseDeleteLesson> response) {
    try {
      String lessonId = request.getLessonId();
      Optional<LessonEntity> lesson = lessonRepository.findById(lessonId);
      if (lesson.isEmpty())
        throw new NoSuchElementException(
            "No existing lesson with id of: " + lessonId);

      lessonRepository.delete(lesson.get());

      response.onNext(ResponseDeleteLesson.newBuilder()
          .setStatus(ResponseDeleteLesson.Status.OK)
          .setMessage("Lesson deleted successfully").build());
      response.onCompleted();
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that gets all lessons from the database
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  public void createLesson(RequestCreateLesson request,
      StreamObserver<ResponseCreateLesson> response) {

    String classId = request.getClassId();
    LessonEntity lesson = new LessonEntity(request.getLesson().getDate(),
        request.getLesson().getTopic(), request.getLesson().getDescription());
    try {

      Optional<ClassEntity> existingClass = classRepository.findById(classId);
      if (existingClass.isEmpty())
        throw new NoSuchElementException(
            "No existing class with id " + classId);

      LessonEntity savedLesson = lessonRepository.save(lesson);
      existingClass.get().addLesson(savedLesson);
      classRepository.save(existingClass.get());

      response.onNext(ResponseCreateLesson.newBuilder().setLesson(
          LessonData.newBuilder().setId(savedLesson.getId())
              .setDate(savedLesson.getDate())
              .setDescription(savedLesson.getDescription())
              .setTopic(savedLesson.getTopic()))
          .build());
      response.onCompleted();

    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that gets all lessons from the database
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  public void updateLesson(RequestUpdateLesson request,
      StreamObserver<ResponseUpdateLesson> response) {
    String lessonId = request.getId();
    try {
      Optional<LessonEntity> currentLesson = lessonRepository.findById(
          lessonId);
      if (currentLesson.isEmpty())
        throw new NoSuchElementException(
            "No existing lesson with id " + lessonId);

      currentLesson.get().setDate(request.getLesson().getDate());
      currentLesson.get().setTopic(request.getLesson().getTopic());
      currentLesson.get().setDescription(request.getLesson().getDescription());

      lessonRepository.save(currentLesson.get());

      response.onNext(ResponseUpdateLesson.newBuilder()
          .setStatus(ResponseUpdateLesson.Status.OK)
          .setMessage("Lesson deleted successfully").build());
      response.onCompleted();
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }

  }
}