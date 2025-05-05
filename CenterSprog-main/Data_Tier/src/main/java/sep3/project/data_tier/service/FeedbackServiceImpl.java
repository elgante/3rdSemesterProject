package sep3.project.data_tier.service;

import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import org.springframework.beans.factory.annotation.Autowired;
import sep3.project.data_tier.entity.FeedbackEntity;
import sep3.project.data_tier.entity.HandInHomeworkEntity;
import sep3.project.data_tier.mappers.FeedbackMapper;
import sep3.project.data_tier.repository.IFeedbackRepository;
import sep3.project.data_tier.repository.IHandInHomeworkRepository;
import sep3.project.protobuf.*;

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
public class FeedbackServiceImpl
    extends FeedbackServiceGrpc.FeedbackServiceImplBase {
  private IFeedbackRepository feedbackRepository;

  private IHandInHomeworkRepository handInHomeworkRepository;
  private FeedbackMapper feedbackMapper = FeedbackMapper.INSTANCE;

  /**
   * 2-argument constructor for FeedbackServiceImpl
   * 
   * @param feedbackRepository       - repository for feedback
   * @param handInHomeworkRepository - repository for hand in homework
   */
  @Autowired
  public FeedbackServiceImpl(IFeedbackRepository feedbackRepository,
      IHandInHomeworkRepository handInHomeworkRepository) {
    this.feedbackRepository = feedbackRepository;

    this.handInHomeworkRepository = handInHomeworkRepository;
  }

  /**
   * Method that adds feedback to the database
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  public void addFeedback(RequestAddFeedback request,
      StreamObserver<Feedback> response) {
    String handInId = request.getHandInId();
    String studentUsername = request.getStudentUsername();
    FeedbackEntity feedback = new FeedbackEntity(request.getFeedback().getGrade(), request.getFeedback().getComment());
    try {
      Optional<HandInHomeworkEntity> optionalHandInHomework = handInHomeworkRepository.findByIdAndUser_Username(
          handInId, studentUsername);

      if (optionalHandInHomework.isPresent()) {
        HandInHomeworkEntity handInHomework = optionalHandInHomework.get();

        if (handInHomework.getFeedback() != null)
          throw new Exception("Feedback for this homework has already been submitted.");

        FeedbackEntity savedFeedback = feedbackRepository.save(feedback);
        handInHomework.setFeedback(savedFeedback);
        handInHomeworkRepository.save(handInHomework);

        response.onNext(feedbackMapper.toProto(feedback).toBuilder().build());
        response.onCompleted();
      } else {
        throw new NoSuchElementException(
            "No existing hand in with id " + handInId + " submitted by user with username " + studentUsername);
      }
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that gets feedback by hand in id and student username
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  public void getFeedbackByHandInIdAndStudentUsername(
      RequestGetFeedbackByHandInIdAndStudentUsername request,
      StreamObserver<ResponseGetFeedback> response) {
    try {
      String handInId = request.getHandInId();
      String studentUsername = request.getStudentUsername();

      Optional<HandInHomeworkEntity> optionalHandInHomework = handInHomeworkRepository.findByIdAndUser_Username(
          handInId, studentUsername);

      if (optionalHandInHomework.isPresent()) {
        HandInHomeworkEntity handInHomework = optionalHandInHomework.get();
        FeedbackEntity feedback = handInHomework.getFeedback();

        if (feedback == null)
          throw new NoSuchElementException(
              "No existing hand in with id " + handInId + " submitted by user with username " + studentUsername);
        System.out.println(feedback);

        ResponseGetFeedback responseMessage = ResponseGetFeedback.newBuilder()
            .setFeedback(feedbackMapper.toProto(feedback)).build();

        response.onNext(responseMessage);
        response.onCompleted();
      } else {
        throw new NoSuchElementException(
            "No existing hand in with id " + handInId + " submitted by user with username " + studentUsername);
      }
    } catch (Exception e) {
      System.out.println(e.getMessage() + " " + e.getStackTrace());
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }
}