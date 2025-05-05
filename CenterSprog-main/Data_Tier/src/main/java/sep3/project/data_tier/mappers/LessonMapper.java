package sep3.project.data_tier.mappers;

import org.mapstruct.Mapper;
import org.mapstruct.factory.Mappers;
import sep3.project.data_tier.entity.LessonEntity;
import sep3.project.protobuf.LessonAttended;
import sep3.project.protobuf.LessonOverview;
import sep3.project.protobuf.LessonData;

@Mapper
public interface LessonMapper {
    LessonMapper INSTANCE = Mappers.getMapper(LessonMapper.class);

    LessonData toProto(LessonEntity lessonEntity);

    LessonOverview toOverviewProto(LessonEntity lessonEntity);

    LessonAttended toAttendandedProto(LessonEntity lessonEntity);

    LessonEntity toEntity(LessonData lesson);

}
