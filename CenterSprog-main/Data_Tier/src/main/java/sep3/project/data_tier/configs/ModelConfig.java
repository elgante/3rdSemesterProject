package sep3.project.data_tier.configs;

import org.springframework.boot.CommandLineRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import sep3.project.data_tier.entity.ClassEntity;
import sep3.project.data_tier.entity.HomeworkEntity;
import sep3.project.data_tier.entity.LessonEntity;
import sep3.project.data_tier.entity.UserEntity;
import sep3.project.data_tier.repository.*;

@Configuration
public class ModelConfig {
    @Bean
    CommandLineRunner productCommandLineRunner(
            IClassRepository classRepository,
            IUserRepository userRepository,
            ILessonRepository lessonRepository,
            IHomeworkRepository homeworkRepository
    ){
        return args -> {
//            set up model here
            UserEntity a1 = new UserEntity("admin","admin","Bob","Builder","bob.builder@gmail.com","admin");
            UserEntity s1 = new UserEntity("damian","damian","Damian","Trafialek","damian.trafialek@gmail.com","student");
            UserEntity t1 = new UserEntity("steffan","steffan","Steffan","Visenberg","sva@via.dk","teacher");
            UserEntity s2 = new UserEntity("julija","julija","Julija","Gramovica","julijagr@gmail.com","student");
            UserEntity t2 = new UserEntity("joseph","steffan","Joseph","Okika","joseph@via.dk","teacher");
            UserEntity t3 = new UserEntity("jakob","jakob","Jakob","Lalassen","jakob@via.dk","teacher");



            LessonEntity l1 = new LessonEntity(133454545665594012l,"Danish traditions","Some of the most beautiful danish tradtions");
            LessonEntity l2 = new LessonEntity(133454545665594012l,"Danish verbs","Verbs in their past present and future forms. So how to speak fluently as a foreigner.");
            LessonEntity l3 = new LessonEntity(133454545665594012l,"Danish curicullum","How to preapare yourself for the exam.");


            ClassEntity c1 = new ClassEntity("danish-module-3","c05.16b");
            ClassEntity c2 = new ClassEntity("danish-module-2","c05.14b");
            ClassEntity c3 = new ClassEntity("danish-module-4","kamtjatka");

            HomeworkEntity h1 = new HomeworkEntity(133454945665594012l,"How long are you learning Danish , Q&A .","Try to give a short answer.");


            l1.addHomework(h1);

            c1.addUser(s1);
            c1.addUser(t1);
            c1.addUser(t2);

            c2.addUser(s2);
            c2.addUser(t2);
            c2.addUser(t3);

            c1.addUser(s1);
            c2.addUser(s1);

            c1.addLesson(l1);
            c1.addLesson(l2);


            userRepository.save(a1);
            userRepository.save(t1);
            userRepository.save(t2);
            userRepository.save(t3);
            userRepository.save(s1);
            userRepository.save(s2);

            homeworkRepository.save(h1);

            lessonRepository.save(l1);
            lessonRepository.save(l2);

            classRepository.save(c1);
            classRepository.save(c2);
            classRepository.save(c3);



        };
    }
}
