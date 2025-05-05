package sep3.project.data_tier.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.project.data_tier.entity.ClassEntity;
import sep3.project.data_tier.entity.UserEntity;

import java.util.List;
import java.util.Optional;

@Repository
public interface IClassRepository extends JpaRepository<ClassEntity, String>
{
  List<ClassEntity> findByUsers_Username(String username);
  List<ClassEntity> findByUsersContains(UserEntity userEntity);

}
