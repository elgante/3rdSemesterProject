package sep3.project.data_tier.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.project.data_tier.entity.HandInHomeworkEntity;
import sep3.project.data_tier.entity.HandInHomeworkEntity;

import java.util.List;
import java.util.Optional;
@Repository
public interface IHandInHomeworkRepository extends JpaRepository<HandInHomeworkEntity, String>
{
  List<HandInHomeworkEntity> findByHomeworkId(String homeworkId);
  Optional<HandInHomeworkEntity> findByIdAndUser_Username(String handInId, String studentUsername);
  Optional<HandInHomeworkEntity> findByHomework_IdAndUser_Username(String homeworkId, String studentUsername);

}
