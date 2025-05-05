package sep3.project.data_tier.entity;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import org.hibernate.annotations.UuidGenerator;

@Entity
@Table
public class FeedbackEntity {
	@Id
	@Column
	@UuidGenerator
	private String id;
	@Column
	private int grade;
	@Column
	private String comment;

	public FeedbackEntity() {
	}

	public FeedbackEntity(int grade, String comment) {
		this.grade = grade;
		this.comment = comment;
	}

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public int getGrade() {
		return grade;
	}

	public void setGrade(int grade) {
		this.grade = grade;
	}

	public String getComment() {
		return comment;
	}

	public void setComment(String comment) {
		this.comment = comment;
	}

	@Override
	public String toString() {
		return "Feedback{" +
				"id='" + id + '\'' +
				", grade=" + grade +
				", comment='" + comment + '\'' +
				'}';
	}
}
