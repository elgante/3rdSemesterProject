package sep3.project.data_tier.mappers;

import org.mapstruct.Mapper;
import org.mapstruct.factory.Mappers;
import sep3.project.data_tier.entity.FeedbackEntity;
import sep3.project.protobuf.Feedback;

@Mapper
public interface FeedbackMapper
{
  FeedbackMapper INSTANCE = Mappers.getMapper(FeedbackMapper.class);
  Feedback toProto(FeedbackEntity feedbackEntity);
  FeedbackEntity toEntity(Feedback feedback);

}
