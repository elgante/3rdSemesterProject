package sep3.project.data_tier.service;

import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;
import org.springframework.beans.factory.annotation.Autowired;
import sep3.project.data_tier.entity.UserEntity;
import sep3.project.data_tier.mappers.UserMapper;
import sep3.project.data_tier.repository.IUserRepository;
import sep3.project.protobuf.*;

import java.util.List;
import java.util.NoSuchElementException;
import java.util.Optional;
import java.util.Random;

/**
 * Service class that handles the grpc communication.
 * 
 * 
 * @author Team_3
 * @version 1.0
 */
@GrpcService
public class UserServiceImpl
    extends UserServiceGrpc.UserServiceImplBase {
  private UserMapper userMapper = UserMapper.INSTANCE;
  private IUserRepository userRepository;

  /**
   * 1-argument constructor for UserServiceImpl
   * 
   * 
   * @param userRepository - repository for user
   * 
   */
  @Autowired
  public UserServiceImpl(IUserRepository userRepository) {
    this.userRepository = userRepository;
  }

  /**
   * Method that creates a user in the database
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  public void createUser(RequestCreateUser request,
      StreamObserver<ResponseCreateUser> response) {
    try {
      String username = request.getUser().getUsername();
      String password = request.getUser().getPassword();
      Optional<UserEntity> user = userRepository.getByUsername(username);
      if (user.isPresent())
        throw new Exception(
            "User with given username already exists. Please try again.");

      UserEntity newUser = new UserEntity(username, password,
          request.getUser().getFirstName(), request.getUser().getLastName(),
          request.getUser().getEmail(), request.getUser().getRole());

      userRepository.save(newUser);
      response.onNext(ResponseCreateUser.newBuilder().setUser(
          UserData.newBuilder().setUsername(newUser.getUsername())
              .setPassword(newUser.getPassword()).setEmail(newUser.getEmail())
              .setFirstName(newUser.getFirstName())
              .setLastName(newUser.getLastName()).setRole(newUser.getRole())
              .build())
          .build());
      response.onCompleted();
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that gets a user by username from the database
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  public void getUserByUsername(RequestUserGetByUsername request,
      StreamObserver<ResponseUserGetByUsername> response) {
    String username = request.getUsername();
    try {
      Optional<UserEntity> existingUser = userRepository.getByUsername(
          username);

      if (existingUser.isEmpty())
        throw new Exception(
            "No existing user with username " + username);
      else
        response.onNext(ResponseUserGetByUsername.newBuilder().setUser(
            UserData.newBuilder().setUsername(existingUser.get().getUsername())
                .setPassword(existingUser.get().getPassword())
                .setEmail(existingUser.get().getEmail())
                .setFirstName(existingUser.get().getFirstName())
                .setLastName(existingUser.get().getLastName())
                .setRole(existingUser.get().getRole()).build())
            .build());
      response.onCompleted();
    } catch (Exception e) {
      Status status = Status.INTERNAL.withDescription(e.getMessage());
      response.onError(status.asRuntimeException());
    }
  }

  /**
   * Method that gets a user by username and password from the database
   * 
   * @param request  - request from client
   * @param response - response from server
   * @throws NoSuchElementException - if no element is found
   */
  @Override
  public void getAllUsers(com.google.protobuf.Empty request,
      StreamObserver<ResponseUserGetAllUsers> response) {
    List<UserEntity> users = userRepository.findAll();

    ResponseUserGetAllUsers responseData = ResponseUserGetAllUsers.newBuilder()
        .buildPartial();
    for (UserEntity user : users) {
      if (!user.getRole().equals("admin"))
        responseData = responseData.toBuilder()
            .addUsers(userMapper.toProto(user)).buildPartial();
    }

    response.onNext(responseData.toBuilder().build());
    response.onCompleted();
  }

}
