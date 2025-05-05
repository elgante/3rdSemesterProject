package sep3.project.data_tier.service;

import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import org.hibernate.Hibernate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.transaction.annotation.Transactional;
import sep3.project.data_tier.entity.ClassEntity;
import sep3.project.data_tier.entity.LessonEntity;
import sep3.project.data_tier.entity.UserEntity;
import sep3.project.data_tier.mappers.ClassMapper;
import sep3.project.data_tier.mappers.LessonMapper;
import sep3.project.data_tier.mappers.UserMapper;
import sep3.project.data_tier.repository.IClassRepository;
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
public class ClassServiceImpl
    extends ClassEntityServiceGrpc.ClassEntityServiceImplBase {
  private IClassRepository classRepository;
  private IUserRepository userRepository;
  private ClassMapper classMapper = ClassMapper.INSTANCE;
  private UserMapper userMapper = UserMapper.INSTANCE;
  private LessonMapper lessonMapper = LessonMapper.INSTANCE;

  /**
   * 2-argument constructor receiving the injection parameters that handles the
   * databse CRUD operations
   * 
   * @param classRepository - handles class repository
   * @param userRepository  - handles user repository
   */
  @Autowired
  public ClassServiceImpl(IClassRepository classRepository,
      IUserRepository userRepository) {
    this.classRepository = classRepository;
    this.userRepository = userRepository;
  }

  /**
   * Method that handles the grpc communication for getting a class entity by id
   * 
   * @param request  - the grpc request
   * @param response - the grpc response
   * @throws NoSuchElementException - if the class entity does not exist
   * 
   */
  @Override
  @Transactional
  public void getClassEntityById(RequestGetClassEntity request, StreamObserver<ResponseGetClassEntity> response) {
    try {
      String id = request.getClassId();
      Optional<ClassEntity> existingClass = classRepository.findById(id);

      if (existingClass.isEmpty())
        throw new NoSuchElementException("No existing class with id " + id);

      Hibernate.initialize(existingClass.get());

      ClassData grpcClass = ClassData.newBuilder()
          .setId(existingClass.get().getId())
          .setTitle(existingClass.get().getTitle())
          .setRoom(existingClass.get().getRoom()).buildPartial();

      if (!existingClass.get().getLessons().isEmpty())
        for (LessonEntity lessonEntity : existingClass.get().getLessons())
          grpcClass = grpcClass.toBuilder()
              .addLessons(lessonMapper.toOverviewProto(lessonEntity))
              .buildPartial();

      grpcClass.toBuilder().build();

      response.onNext(
          ResponseGetClassEntity.newBuilder().setClassEntity(grpcClass)
              .build());
      response.onCompleted();

    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that handles the grpc communication for getting all class entities
   * 
   * @param request  - the grpc request
   * @param response - the grpc response
   * @throws NoSuchElementException - if the class entity does not exist
   * 
   */
  @Override
  public void getClassEntities(RequestGetClassEntities request,
      StreamObserver<ResponseGetClassEntities> response) {
    try {
      List<ClassEntity> classes;

      if (request.hasUsername()) {
        String username = request.getUsername();
        Optional<UserEntity> user = userRepository.getByUsername(username);
        if (user.isEmpty())
          throw new NoSuchElementException(
              "No existing user with username " + username);
        classes = classRepository.findByUsers_Username(username);
      } else {
        classes = classRepository.findAll();
      }

      List<ClassData> grpcsClasses = new ArrayList<>();
      for (ClassEntity entity : classes) {
        ClassData grpcClass = ClassData.newBuilder().setId(entity.getId())
            .setTitle(entity.getTitle()).setRoom(entity.getRoom())
            .buildPartial();
        for (UserEntity userEntity : entity.getUsers())
          grpcClass = grpcClass.toBuilder()
              .addParticipants(userMapper.toParticipantProto(userEntity))
              .buildPartial();

        grpcClass.toBuilder().build();
        grpcsClasses.add(grpcClass);
      }
      ResponseGetClassEntities responseMessage = ResponseGetClassEntities.newBuilder()
          .addAllClassEntities(grpcsClasses).build();

      response.onNext(responseMessage);
      response.onCompleted();
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that handles the grpc communication for updating a class entity
   * 
   * @param request  - the grpc request
   * @param response - the grpc response
   * @throws NoSuchElementException - if the class entity does not exist
   * 
   */
  @Override
  @Transactional
  public void getClassParticipants(
      RequestGetClassParticipants request,
      StreamObserver<ResponseGetClassParticipants> response) {
    try {
      String id = request.getClassId();
      Optional<ClassEntity> existingClass = classRepository.findById(id);

      if (existingClass.isEmpty())
        throw new NoSuchElementException("No existing class with id " + id);

      Hibernate.initialize(existingClass);

      List<UserParticipant> participants = new ArrayList<>();
      for (UserEntity entity : existingClass.get().getUsers()) {
        if (request.hasRole() && !entity.getRole().equals(request.getRole())) {
          continue;
        }

        UserParticipant grpcParticipant = userMapper.toParticipantProto(entity).toBuilder().build();

        participants.add(grpcParticipant);
      }

      response.onNext(ResponseGetClassParticipants.newBuilder()
          .addAllParticipants(participants).build());
      response.onCompleted();
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that handles the grpc communication for updating a class entity
   * 
   * @param request  - the grpc request
   * @param response - the grpc response
   * @throws NoSuchElementException - if the class entity does not exist
   * 
   */
  @Override
  public void createClassEntity(RequestCreateClassEntity request,
      StreamObserver<ResponseCreateClassEntity> response) {
    String title = request.getClassEntityCreation().getTitle();
    String room = request.getClassEntityCreation().getRoom();

    try {

      ClassEntity createdClass = new ClassEntity(title, room);
      classRepository.save(createdClass);

      response.onNext(ResponseCreateClassEntity.newBuilder()
          .setClassEntity(classMapper.toProto(createdClass)).build());
      response.onCompleted();
    } catch (Exception e) {
      response.onError(Status.INTERNAL.withDescription(
          "Error creating class : " + e.getMessage()).asRuntimeException());
    }
  }

  /**
   * Method that handles the grpc communication for updating a class entity
   * 
   * @param request  - the grpc request
   * @param response - the grpc response
   * @throws NoSuchElementException - if the class entity does not exist
   */
  @Override
  @Transactional
  public void getClassAttendance(
      RequestGetClassAttendance request,
      StreamObserver<ResponseGetClassAttendance> response) {
    try {
      String id = request.getClassId();
      Optional<ClassEntity> existingClass = classRepository.findById(id);

      if (existingClass.isEmpty())
        throw new NoSuchElementException("No existing class with id " + id);

      Hibernate.initialize(existingClass);

      List<LessonAttendance> lessonsAttendance = new ArrayList<>();
      if (!existingClass.get().getLessons().isEmpty()) {
        for (LessonEntity lessonEntity : existingClass.get().getLessons()) {
          Set<UserEntity> usersInAttendance = lessonEntity.getAttendance();
          for (UserEntity userEntity : usersInAttendance) {
            if (!userEntity.getRole().equals("student")) {
              continue;
            }
            LessonAttendance lessonAttendance = LessonAttendance.newBuilder()
                .setId(lessonEntity.getId())
                .addParticipants(userMapper.toParticipantProto(userEntity))
                .build();
            lessonsAttendance.add(lessonAttendance);
          }
        }
      }

      response.onNext(ResponseGetClassAttendance.newBuilder()
          .addAllLessonsAttendance(lessonsAttendance).build());
      response.onCompleted();
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that handles the grpc communication for updating a class entity
   * 
   * @param request  - the grpc request
   * @param response - the grpc response
   * @throws NoSuchElementException - if the class entity does not exist
   */
  @Override
  @Transactional
  public void getClassAttendanceByUsername(
      RequestGetClassAttendanceByUsername request,
      StreamObserver<ResponseGetClassAttendanceByUsername> response) {
    try {
      String id = request.getClassId();
      Optional<ClassEntity> existingClass = classRepository.findById(id);

      if (existingClass.isEmpty())
        throw new IllegalStateException("No existing class with id " + id);

      Hibernate.initialize(existingClass);

      String username = request.getUsername();
      Optional<UserEntity> user = userRepository.getByUsername(username);
      if (user.isEmpty())
        throw new NoSuchElementException(
            "No existing user with username " + username);

      List<LessonAttended> lessonsAttended = new ArrayList<>();
      Set<LessonEntity> lessons = existingClass.get().getLessons();
      if (!lessons.isEmpty()) {
        for (LessonEntity lessonEntity : lessons) {
          if (lessonEntity.getAttendance().stream().anyMatch(
              userEntity -> userEntity.getUsername()
                  .equals(username))) {
            lessonsAttended.add(lessonMapper.toAttendandedProto(lessonEntity));
          }
        }
      }

      response.onNext(ResponseGetClassAttendanceByUsername.newBuilder()
          .addAllLessons(lessonsAttended).build());
      response.onCompleted();

    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that handles the grpc communication for updating a class entity
   * 
   * @param request  - the grpc request
   * @param response - the grpc response
   * @throws NoSuchElementException - if the class entity does not exist
   */
  @Override
  public void updateParticipants(
      RequestUpdateClassParticipants request,
      StreamObserver<ResponseUpdateClassParticipants> response) {
    String classId = request.getId();
    List<String> participantsUsernames = request.getParticipantsUsernamesList();
    try {
      Optional<ClassEntity> existingClass = classRepository.findById(classId);
      if (existingClass.isEmpty())
        throw new NoSuchElementException(
            "No existing class with id " + classId);

      Set<UserEntity> newParticipants = userRepository.findAll().stream()
          .filter(user -> participantsUsernames.contains(user.getUsername()))
          .collect(Collectors.toSet());

      existingClass.get().setUsers(newParticipants);
      classRepository.save(existingClass.get());

      response.onNext(
          ResponseUpdateClassParticipants.newBuilder().setResult(true).build());
      response.onCompleted();
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }
}
