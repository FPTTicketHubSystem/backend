# Project Title: Event Ticket Supply Management Platform for FPT University Da Nang

## Project Overview

This project aims to develop a web platform for managing event ticket supply at FPT University Da Nang. The platform will leverage ASP.NET Core Web API for the backend, ReactJS for the frontend, and SQL Server for the database.

## Objectives

- Develop a scalable and maintainable web platform for managing event ticket supply.
- Ensure a user-friendly interface for students and staff to book and manage event tickets.
- Implement robust authentication and authorization mechanisms.
- Provide real-time updates and notifications for ticket availability and event changes.

## Scope

- **Frontend**: ReactJS application for users to interact with the platform.
- **Backend**: ASP.NET Core Web API to handle business logic and data management.
- **Database**: SQL Server to store and manage event and ticket information.
- **Authentication & Authorization**: Secure login and role-based access control.
- **Notifications**: Real-time updates and email notifications for users.

## Task Board (Scrum)

### Product Backlog

| Task ID | Description                                  | Priority | Status    |
|---------|----------------------------------------------|----------|-----------|
| 1       | Setup project repository                     | High     | Done      |
| 2       | Design database schema                       | High     | In Progress|
| 3       | Develop user authentication module           | High     | Pending   |
| 4       | Create React components for ticket booking   | Medium   | Pending   |
| 5       | Implement API for ticket management          | High     | Pending   |
| 6       | Setup CI/CD pipeline                         | Medium   | Pending   |
| 7       | Testing and QA                               | High     | Pending   |

### Sprint 1

| Task ID | Description                                  | Assignee     | Priority | Status    | Start Date | Due Date   |
|---------|----------------------------------------------|--------------|----------|-----------|------------|------------|
| 1       | Setup project repository                     | Your Name    | High     | Done      | 2023-08-01 | 2023-08-02 |
| 2       | Design database schema                       | Developer 1  | High     | In Progress| 2023-08-03 | 2023-08-07 |
| 3       | Develop user authentication module           | Developer 2  | High     | Pending   | 2023-08-08 | 2023-08-12 |

### Sprint 2

| Task ID | Description                                  | Assignee     | Priority | Status    | Start Date | Due Date   |
|---------|----------------------------------------------|--------------|----------|-----------|------------|------------|
| 4       | Create React components for ticket booking   | Developer 1  | Medium   | Pending   | 2023-08-13 | 2023-08-17 |
| 5       | Implement API for ticket management          | Developer 2  | High     | Pending   | 2023-08-13 | 2023-08-19 |
| 6       | Setup CI/CD pipeline                         | DevOps       | Medium   | Pending   | 2023-08-20 | 2023-08-25 |

## Milestones

| Milestone ID | Description                                   | Due Date   | Status    |
|--------------|-----------------------------------------------|------------|-----------|
| 1            | Initial project setup complete                | 2023-08-02 | Completed |
| 2            | Database schema design complete               | 2023-08-07 | In Progress|
| 3            | User authentication module development complete| 2023-08-12 | Pending   |
| 4            | React components for ticket booking complete  | 2023-08-17 | Pending   |
| 5            | API for ticket management complete            | 2023-08-19 | Pending   |
| 6            | CI/CD pipeline setup complete                 | 2023-08-25 | Pending   |
| 7            | MVP ready for internal testing                | 2023-09-01 | Pending   |

## Meeting Notes

### Sprint Planning Meeting (2023-08-01)

**Attendees:** Your Name, Developer 1, Developer 2, DevOps, QA

**Agenda:**
- Review and prioritize tasks for Sprint 1
- Assign tasks to team members
- Set deadlines for Sprint 1 tasks

**Notes:**
- Project repository setup is high priority.
- Database schema design should start immediately.
- Authentication module development planned for next week.

### Sprint Review Meeting (2023-08-08)

**Attendees:** Your Name, Developer 1, Developer 2, DevOps, QA

**Agenda:**
- Review completed tasks in Sprint 1
- Discuss challenges faced during the sprint
- Plan for Sprint 2

**Notes:**
- Project repository setup completed successfully.
- Database schema design is in progress, expected to complete on time.
- Authentication module development to begin tomorrow.

## Issues and Risks

| Issue ID | Description                                    | Severity | Status    | Mitigation Plan                |
|----------|------------------------------------------------|----------|-----------|--------------------------------|
| 1        | Delay in database schema design                | Medium   | Open      | Adjust timeline, reassign tasks|
| 2        | Authentication module integration issues       | High     | Pending   | Conduct code reviews, add tests|

## Useful Links

- [Project Repository](https://github.com/FPTTicketHubSystem)
- [Design Mockups](https://www.figma.com/file/XXXXXX/Project-Designs)
- [API Documentation](https://project-docs.com/api)
- [Check payment](https://payos.vn/)

---

### Notes

- Regularly update this README.md to reflect the current state of the project.
- Use GitHub Issues to track detailed tasks and bugs.
- Utilize GitHub Projects for more advanced project tracking and Kanban boards.

````javascript
import React, { useState } from 'react';

function TodoForm({ addTodo }) {
  const [value, setValue] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!value) return;
    addTodo(value);
    setValue('');
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        value={value}
        onChange={(e) => setValue(e.target.value)}
        placeholder="Add a new todo"
      />
      <button type="submit">Add</button>
    </form>
  );
}

export default TodoForm;
````
