package sep3.project.data_tier.service;

import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import org.springframework.beans.factory.annotation.Autowired;
import sep3.project.data_tier.entity.HandInHomeworkEntity;
import sep3.project.data_tier.entity.HomeworkEntity;
import sep3.project.data_tier.entity.UserEntity;
import sep3.project.data_tier.repository.IHandInHomeworkRepository;
import sep3.project.data_tier.repository.IHomeworkRepository;
import sep3.project.data_tier.repository.IUserRepository;
import sep3.project.protobuf.*;

import java.util.ArrayList;
import java.util.List;
import java.util.NoSuchElementException;
import java.util.Optional;

/**
 * Service class that handles the grpc communication.
 * 
 * 
 * @author Team_3
 * @version 1.0
 */
@GrpcService
public class HandInHomeworkServiceImpl
    extends HandInHomeworkServiceGrpc.HandInHomeworkServiceImplBase {
  private IHandInHomeworkRepository handInHomeworkRepository;
  private IUserRepository userRepository;
  private IHomeworkRepository homeworkRepository;

  /**
   * 3-argument constructor for HandInHomeworkServiceImpl
   * 
   * @param handInHomeworkRepository - repository for hand in homework
   * @param userRepository           - repository for user
   * @param homeworkRepository       - repository for homework
   */
  @Autowired
  public HandInHomeworkServiceImpl(
      IHandInHomeworkRepository handInHomeworkRepository,
      IUserRepository userRepository, IHomeworkRepository homeworkRepository) {
    this.handInHomeworkRepository = handInHomeworkRepository;
    this.userRepository = userRepository;
    this.homeworkRepository = homeworkRepository;
  }

  /**
   * Method that adds hand in homework to the database
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  public void handInHomework(RequestCreateHandInHomework request,
      StreamObserver<ResponseGetHandInHomework> response) {
    String homeworkId = request.getHomeworkId();
    String studentUsername = request.getStudentUsername();

    try {
      HandInHomeworkEntity handInHomework = new HandInHomeworkEntity();

      Optional<UserEntity> user = userRepository.findById(studentUsername);
      if (user.isEmpty())
        throw new NoSuchElementException(
            "No existing user with username " + studentUsername);

      Optional<HomeworkEntity> homework = homeworkRepository.findById(homeworkId);
      if (homework.isEmpty())
        throw new NoSuchElementException(
            "No existing homework with id " + homeworkId);

      handInHomework.setAnswer(request.getAnswer());
      handInHomework.setUser(user.get());
      handInHomework.setHomework(homework.get());

      HandInHomeworkEntity savedHandInHomework = handInHomeworkRepository.save(
          handInHomework);

      response.onNext(ResponseGetHandInHomework.newBuilder()
          .setHandInHomework(
              HandInHomework.newBuilder().setId(savedHandInHomework.getId())
                  .setAnswer(savedHandInHomework.getAnswer())
                  .setStudentUsername(user.get().getUsername()).build())
          .build());
      response.onCompleted();
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that gets hand ins by homework id
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  public void getHandInsByHomeworkId(
      RequestGetHandInsByHomeworkId request,
      StreamObserver<ResponseGetHandInsByHomeworkId> response) {
    String homeworkId = request.getHomeworkId();
    try {
      Optional<HomeworkEntity> homework = homeworkRepository.findById(homeworkId);
      if (homework.isEmpty())
        throw new NoSuchElementException(
            "No existing homework with id " + homeworkId);

      List<HandInHomeworkEntity> handIns = handInHomeworkRepository.findByHomeworkId(
          homeworkId);
      List<HandInHomework> handInMessages = new ArrayList<>();

      for (HandInHomeworkEntity entity : handIns) {
        UserEntity user = entity.getUser();
        String studentUsername = user.getUsername();

        HandInHomework message = HandInHomework.newBuilder()
            .setId(entity.getId()).setAnswer(entity.getAnswer())
            .setStudentUsername(studentUsername).build();

        if (entity.getFeedback() != null)
          message = message.toBuilder().setFeedback(
              Feedback.newBuilder().setComment(entity.getFeedback().getComment())
                  .setGrade(entity.getFeedback().getGrade()).build())
              .build();

        handInMessages.add(message);
      }

      response.onNext(ResponseGetHandInsByHomeworkId.newBuilder()
          .addAllHandIns(handInMessages).build());
      response.onCompleted();
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that gets hand in by homework id and student username
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  public void getHandInByHomeworkIdAndStudentUsername(
      RequestGetHandInByHomeworkIdAndStudentUsername request,
      StreamObserver<ResponseGetHandInHomework> response) {
    String homeworkId = request.getHomeworkId();
    String studentUsername = request.getStudentUsername();

    try {
      Optional<UserEntity> user = userRepository.findById(studentUsername);
      if (user.isEmpty())
        throw new NoSuchElementException(
            "No existing user with username " + studentUsername);

      Optional<HomeworkEntity> homework = homeworkRepository.findById(homeworkId);
      if (homework.isEmpty())
        throw new NoSuchElementException(
            "No existing homework with id " + homeworkId);

      Optional<HandInHomeworkEntity> handInHomework = handInHomeworkRepository.findByHomework_IdAndUser_Username(
          homeworkId, studentUsername);

      if (handInHomework.isPresent()) {
        HandInHomeworkEntity entity = handInHomework.get();

        HandInHomework handInMessage = HandInHomework.newBuilder()
            .setId(entity.getId()).setAnswer(entity.getAnswer())
            .setStudentUsername(user.get().getUsername()).buildPartial();

        if (entity.getFeedback() != null)
          handInMessage = handInMessage.toBuilder().setFeedback(
              Feedback.newBuilder().setGrade(entity.getFeedback().getGrade())
                  .setComment(entity.getFeedback().getComment())
                  .build())
              .build();

        response.onNext(ResponseGetHandInHomework.newBuilder()
            .setHandInHomework(handInMessage).build());
        response.onCompleted();
      } else {
        throw new NoSuchElementException(
            "No existing hand in with homework id " + homeworkId + " submitted by user with username "
                + studentUsername);
      }
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }
}
