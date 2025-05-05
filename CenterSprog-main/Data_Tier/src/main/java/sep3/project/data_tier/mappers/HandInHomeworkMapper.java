package sep3.project.data_tier.mappers;

import org.mapstruct.Mapper;
import org.mapstruct.factory.Mappers;
import sep3.project.data_tier.entity.HandInHomeworkEntity;
import sep3.project.protobuf.HandInHomework;

@Mapper
public interface HandInHomeworkMapper
{
  HandInHomeworkMapper INSTANCE = Mappers.getMapper(HandInHomeworkMapper.class);
  HandInHomework toProto(HandInHomeworkEntity handInHomeworkEntity);
  HandInHomeworkEntity toEntity(HandInHomework handInHomework);
}
