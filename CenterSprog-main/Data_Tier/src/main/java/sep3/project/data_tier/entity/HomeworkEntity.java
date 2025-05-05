package sep3.project.data_tier.entity;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import org.hibernate.annotations.UuidGenerator;

@Entity
@Table
public class HomeworkEntity {
	@Id
	@Column
	@UuidGenerator
	private String id;
	@Column
	private long deadline;
	@Column
	private String title;
	@Column
	private String description;

	public HomeworkEntity() {
	}

	public HomeworkEntity(long deadline, String title, String description) {
		this.deadline = deadline;
		this.title = title;
		this.description = description;
	}

	public HomeworkEntity(String id, long deadline, String title, String description) {
		this(deadline,title,description);
		this.id = id;
	}

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public long getDeadline() {
		return deadline;
	}

	public void setDeadline(long deadline) {
		this.deadline = deadline;
	}

	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}

	public String getDescription() {
		return description;
	}

	public void setDescription(String description) {
		this.description = description;
	}


	@Override
	public String toString() {
		return "Homework{" +
				"id='" + id + '\'' +
				", deadline=" + deadline +
				", title='" + title + '\'' +
				", description='" + description + '\'' +
				'}';
	}
}
