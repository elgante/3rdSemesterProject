package sep3.project.data_tier.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.project.data_tier.entity.FeedbackEntity;

@Repository
public interface IFeedbackRepository extends JpaRepository<FeedbackEntity, String>
{
}
