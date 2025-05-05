package sep3.project.data_tier.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.project.data_tier.entity.UserEntity;

import java.util.Optional;
@Repository
public interface IUserRepository extends JpaRepository<UserEntity, String>
{
    Optional<UserEntity> getByUsername(String username);
}
