package sep3.project.data_tier.mappers;

import org.mapstruct.Mapper;
import org.mapstruct.factory.Mappers;
import sep3.project.data_tier.entity.ClassEntity;
import sep3.project.protobuf.ClassData;

@Mapper
public interface ClassMapper
{
  ClassMapper INSTANCE = Mappers.getMapper(ClassMapper.class);
  ClassData toProto(ClassEntity classData);
  ClassEntity toEntity(ClassData classEntity);

}
