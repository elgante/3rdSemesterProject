package sep3.project.data_tier.entity;

import jakarta.persistence.*;

@Entity
@Table
public class UserEntity {
	@Id
	@Column
	private String username;


	@Column
	private String password;

	@Column
	private String firstName;

	@Column
	private String lastName;

	@Column
	private String email;

	@Column
	private String role;

	public UserEntity() {
	}

	public UserEntity(String username, String password, String firstName, String lastName, String email, String role) {
		this.username = username;
		this.password = password;
		this.firstName = firstName;
		this.lastName = lastName;
		this.email = email;
		this.role = role;
	}

	public String getUsername() {
		return username;
	}

	public void setUsername(String username) {
		this.username = username;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	public String getFirstName() {
		return firstName;
	}

	public void setFirstName(String firstName) {
		this.firstName = firstName;
	}

	public String getLastName() {
		return lastName;
	}

	public void setLastName(String lastName) {
		this.lastName = lastName;
	}

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getRole() {
		return role;
	}

	public void setRole(String role) {
		this.role = role;
	}
}

