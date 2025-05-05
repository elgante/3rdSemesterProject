package sep3.project.data_tier.mappers;


import org.mapstruct.Mapper;
import org.mapstruct.factory.Mappers;
import sep3.project.data_tier.entity.HomeworkEntity;
import sep3.project.protobuf.Homework;

@Mapper
public interface HomeworkMapper {
    HomeworkMapper INSTANCE = Mappers.getMapper(HomeworkMapper.class);
    Homework toProto(HomeworkEntity homeworkEntity);
    HomeworkEntity toEntity(Homework homework);

}
