# Project Validation and Design Overview  

## Technologies and Architectural Patterns Used  
- **AutoMapper** – Used for object mapping to simplify data transfer between layers.  
- **Repository Pattern** – Implements a clean separation between data access and business logic, promoting maintainability and scalability.  
- **ASP.NET Identity** – Handles authentication and authorization, ensuring secure access control.  
- **Table Per Type (TPT) Inheritance Strategy** – Utilized for efficient database normalization and structured entity relationships.  

---

## Validations Implemented  

### # Schedule Validations  

#### ## Schedule Creation  
- **Duplicate Schedule Prevention** – Ensures that a schedule does not already exist with the same `AvailableFrom`, `AvailableTo`, and `Date`.  
- **Schedule Date Validation** – Ensures that the schedule date is not in the past.  
- **Time Range Validation** – Confirms that `AvailableFrom` is earlier than `AvailableTo`.  

#### ## Schedule Update  
- **Schedule Existence Verification** – Ensures that the schedule exists before updating.  
- **Duplicate Schedule Prevention on Update** – Prevents updates that result in duplicate schedules with identical `AvailableFrom`, `AvailableTo`, and `Date`.  
- **Updated Schedule Date Validation** – Ensures that the updated schedule date is not in the past.  
- **Updated Time Range Validation** – Confirms that `AvailableFrom` is earlier than `AvailableTo`.  

#### ## Schedule Deletion  
- **Schedule Assignment Check Before Deletion** – Ensures that a schedule is not assigned to any staff member before deletion.  

---

### # Staff Validations  

#### ## Staff Creation  
- **Duplicate Staff Prevention** – Ensures that a staff member with the same `Email` or `UserName` does not already exist.  
- **Authentication Handling** – Displays an appropriate error message if registration fails.  

#### ## Staff Update  
- **Staff Existence Verification** – Ensures that the staff member exists before updating.  

#### ## Staff Deletion  
- **Staff Existence Verification Before Deletion** – Prevents deletion of non-existing staff.  
- **Future Appointment Check Before Deletion** – Ensures that a staff member with upcoming appointments cannot be deleted.  
- **Soft Deletion of Assigned Staff Schedules** – If assigned, marks `StaffSchedule` as deleted instead of performing a hard deletion.  

#### ## Staff Search  
- **Empty Search Query Handling** – Prevents empty search queries from proceeding.  
- **Search Result Validation** – Returns a message if no matching doctors are found.  

---

### # Staff Schedule Validations  

#### ## Staff Assignment to Schedule  
- **Reactivation of Previously Deleted Assignments** – If an assignment was deleted before, it is reactivated instead of creating a new one.  
- **Duplicate Active Assignment Prevention** – Ensures that a staff member is not assigned to the same schedule multiple times.  

#### ## Staff Deassignment from Schedule  
- **Assignment Existence Verification Before Deassigning** – Ensures the assignment exists before deassigning.  
- **Upcoming Appointments Check Before Deassigning** – Prevents deassigning a staff member from a schedule if they have upcoming appointments.  

---

### # Appointment Validations  

#### ## Doctor's Appointments  
- Retrieves only non-canceled appointments assigned to the logged-in doctor.  

#### ## Patient's Available Appointments  
- Ensures available schedules exist for the doctor.  
- Validates that the appointment date and time align with the doctor's available schedule.  

#### ## Booking an Appointment  
- Sets the appointment status to `UPCOMING`.  

#### ## Canceling an Appointment  
- Verifies that the appointment exists before cancellation.  
- Ensures that cancellations can only occur at least **24 hours in advance**.  
- Updates the status to `CANCELLED`.  

---

### # Department Validations  
- **Duplicate Department Prevention** – Ensures that a department with the same name does not already exist.  
- **Restoration of Previously Deleted Departments** – If an existing department was previously deleted, it is restored instead of creating a duplicate.  

---

### # Medical History Validations  

#### ## Adding a Medical History Entry  
- **Appointment Existence Check** – Ensures that a medical history entry is only added if the associated appointment exists.  
- **Duplicate Medical History Prevention** – Prevents multiple medical histories for the same appointment.  

#### ## Updating a Medical History  
- **Medical History Existence Check** – Ensures that the medical history record exists before applying updates.  
- **Restricted Updates** – Ensures that updates are only applied if a valid medical history entry is found.  

---

### # Security & General Validations  

#### ## Authentication & Authorization  
- **Role-Based Access Control (RBAC)** – Enforced via `[Authorize(Roles = "...")]`.  

#### ## Data Integrity Checks  
- Ensures that required entities (e.g., appointments, departments, medical histories) exist before operations.  

#### ## Input Validation  
- Utilizes `ModelState.IsValid` before processing requests.  
- Redirects users with error messages if validation fails.  

#### ## Soft Deletion Mechanism  
- Departments are marked as deleted rather than being permanently removed.  
- Ensures that historical data remains intact.  

---

This README provides a structured and formalized overview of the project's validation logic and architectural design. You can add your implementation code under each section as needed. 🚀  
