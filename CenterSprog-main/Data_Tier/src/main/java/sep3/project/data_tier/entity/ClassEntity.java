package sep3.project.data_tier.entity;

import jakarta.persistence.*;
import org.hibernate.annotations.UuidGenerator;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.Set;

@Entity
@Table
public class ClassEntity {

	@Id
	@Column
	@UuidGenerator
	private String id;
	@Column
	private String title;
	@Column
	private String room;
	@ManyToMany(fetch = FetchType.EAGER)
	@JoinTable(name = "user_class", joinColumns = @JoinColumn(name = "class_id"), inverseJoinColumns = @JoinColumn(name = "username"))
	private Set<UserEntity> users = new HashSet<>();

	@OneToMany(fetch = FetchType.EAGER, cascade = CascadeType.REMOVE, orphanRemoval = true)
	@JoinTable(name = "class_lesson", joinColumns = @JoinColumn(name = "class_id"), inverseJoinColumns = @JoinColumn(name = "lesson_id"))
	private Set<LessonEntity> lessons = new HashSet<>();

	public ClassEntity() {
	}

	public ClassEntity(String title, String room) {
		this.title = title;
		this.room = room;
	}

	public void addUser(UserEntity user) {
		users.add(user);
	}

	public void addLesson(LessonEntity lesson) {
		lessons.add(lesson);
	}

	public void removeLesson(String id) {
		lessons.removeIf(lesson -> lesson.getId().equals(id));
	}

	public void removeUser(String username) {
		users.removeIf(user -> user.getUsername().equals(username));
	}

	public void setUsers(Set<UserEntity> users) {
		this.users = users;
	}

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}

	public String getRoom() {
		return room;
	}

	public void setRoom(String room) {
		this.room = room;
	}

	@Override
	public String toString() {
		return "Class{" +
				"id='" + id + '\'' +
				", title='" + title + '\'' +
				", room='" + room + '\'' +
				'}';
	}

	public Set<UserEntity> getUsers() {
		return users;
	}

	public Set<LessonEntity> getLessons() {
		return lessons;
	}
}
