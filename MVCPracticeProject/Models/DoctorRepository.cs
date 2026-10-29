using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCPracticeProject.Models
{
    public class DoctorRepository
    {
        public string connection = "Data Source=.;Initial Catalog=AssignmentDB;User ID=sa;Password=mcc#1234;";
        public List<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();

            using (SqlConnection con = new SqlConnection(connection))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM labTest.Doctors", con))
                {
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            doctors.Add(new Doctor
                            {
                                DoctorId = Convert.ToInt32(reader["doctor_id"]),
                                FirstName = reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                Specialization = reader["specialization"].ToString(),
                                DepartmentId = Convert.ToInt32(reader["department_id"]),
                                Email = reader["email"].ToString(),
                                PhoneNumber = reader["phone_number"].ToString()
                            });
                        }
                    }
                }
            }

            return doctors;
        }

        public Doctor GetSingleDoctorById(int id)
        {
            Doctor doctor = new Doctor();

            using (SqlConnection con = new SqlConnection(connection))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM labTest.Doctors where doctor_id = @id", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@id",id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            doctor = new Doctor
                            {
                                DoctorId = Convert.ToInt32(reader["doctor_id"]),
                                FirstName = reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                Specialization = reader["specialization"].ToString(),
                                DepartmentId = Convert.ToInt32(reader["department_id"]),
                                Email = reader["email"].ToString(),
                                PhoneNumber = reader["phone_number"].ToString()
                            };
                        }
                    }
                }
            }

            return doctor;
        }

        public string DeleteSingleDoctorById(int id)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM labTest.Doctors where doctor_id = @id", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@id", id);

                    int deletedDocCount = cmd.ExecuteNonQuery();
                    if (deletedDocCount > 0)
                    {
                        return $"Doctor with id {id} was found and deleted successfully";
                    }
                    else {
                        return $"Failed to delete doctor with id {id}";
                    }
                }
            }
        }

        public string CreateDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                return "Doctor object is required.";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    string query = @"INSERT INTO labTest.Doctors
                            (
                                first_name,
                                last_name,
                                specialization,
                                department_id,
                                email,
                                phone_number
                            )
                            VALUES
                            (
                                @FirstName,
                                @LastName,
                                @Specialization,
                                @DepartmentId,
                                @Email,
                                @PhoneNumber
                            )";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", doctor.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", doctor.LastName);
                        cmd.Parameters.AddWithValue("@Specialization", doctor.Specialization);
                        cmd.Parameters.AddWithValue("@DepartmentId", doctor.DepartmentId);
                        cmd.Parameters.AddWithValue("@Email", doctor.Email);
                        cmd.Parameters.AddWithValue("@PhoneNumber", doctor.PhoneNumber);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return "Doctor created successfully.";
                        }

                        return "Doctor could not be created.";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                return "Doctor object is required.";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    string query = @"
                UPDATE labTest.Doctors
                SET
                    first_name = CASE
                                    WHEN @FirstName IS NULL OR @FirstName = ''
                                    THEN first_name
                                    ELSE @FirstName
                                 END,

                    last_name = CASE
                                    WHEN @LastName IS NULL OR @LastName = ''
                                    THEN last_name
                                    ELSE @LastName
                                 END,

                    specialization = CASE
                                        WHEN @Specialization IS NULL OR @Specialization = ''
                                        THEN specialization
                                        ELSE @Specialization
                                     END,

                    department_id = CASE
                                        WHEN @DepartmentId <= 0
                                        THEN department_id
                                        ELSE @DepartmentId
                                    END,

                    email = CASE
                                WHEN @Email IS NULL OR @Email = ''
                                THEN email
                                ELSE @Email
                            END,

                    phone_number = CASE
                                        WHEN @PhoneNumber IS NULL OR @PhoneNumber = ''
                                        THEN phone_number
                                        ELSE @PhoneNumber
                                   END

                WHERE doctor_id = @DoctorId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@DoctorId", doctor.DoctorId);
                        cmd.Parameters.AddWithValue("@FirstName", (object)doctor.FirstName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@LastName", (object)doctor.LastName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Specialization", (object)doctor.Specialization ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DepartmentId", doctor.DepartmentId);
                        cmd.Parameters.AddWithValue("@Email", (object)doctor.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PhoneNumber", (object)doctor.PhoneNumber ?? DBNull.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return "Doctor updated successfully.";
                        }

                        return "Doctor not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // REGISTER DOCTOR
        public string RegisterDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                return "Doctor information is required.";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    // Check if email is already taken
                    string checkEmailQuery = "SELECT COUNT(1) FROM labTest.Doctors WHERE email = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkEmailQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", doctor.Email);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            return "A doctor with this email address already exists.";
                        }
                    }

                    // Check if 'password' column exists in labTest.Doctors
                    string colCheckQuery = "SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'labTest' AND TABLE_NAME = 'Doctors' AND COLUMN_NAME = 'password'";
                    bool hasPasswordCol = false;
                    using (SqlCommand colCmd = new SqlCommand(colCheckQuery, con))
                    {
                        hasPasswordCol = colCmd.ExecuteScalar() != null;
                    }

                    string query;
                    if (hasPasswordCol)
                    {
                        query = @"INSERT INTO labTest.Doctors
                                (first_name, last_name, specialization, department_id, email, phone_number, password)
                                VALUES
                                (@FirstName, @LastName, @Specialization, @DepartmentId, @Email, @PhoneNumber, @Password)";
                    }
                    else
                    {
                        query = @"INSERT INTO labTest.Doctors
                                (first_name, last_name, specialization, department_id, email, phone_number)
                                VALUES
                                (@FirstName, @LastName, @Specialization, @DepartmentId, @Email, @PhoneNumber)";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", doctor.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", doctor.LastName);
                        cmd.Parameters.AddWithValue("@Specialization", doctor.Specialization);
                        cmd.Parameters.AddWithValue("@DepartmentId", doctor.DepartmentId);
                        cmd.Parameters.AddWithValue("@Email", doctor.Email);
                        cmd.Parameters.AddWithValue("@PhoneNumber", doctor.PhoneNumber);

                        if (hasPasswordCol)
                        {
                            cmd.Parameters.AddWithValue("@Password", (object)doctor.Password ?? DBNull.Value);
                        }

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            return "Success";
                        }
                        return "Failed to register doctor.";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // VALIDATE LOGIN
        public Doctor ValidateDoctor(string email, string password)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                // Check if 'password' column exists in labTest.Doctors
                string colCheckQuery = "SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'labTest' AND TABLE_NAME = 'Doctors' AND COLUMN_NAME = 'password'";
                bool hasPasswordCol = false;
                using (SqlCommand colCmd = new SqlCommand(colCheckQuery, con))
                {
                    hasPasswordCol = colCmd.ExecuteScalar() != null;
                }

                string query = hasPasswordCol
                    ? "SELECT * FROM labTest.Doctors WHERE email = @Email AND password = @Password"
                    : "SELECT * FROM labTest.Doctors WHERE email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    if (hasPasswordCol)
                    {
                        cmd.Parameters.AddWithValue("@Password", password);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Doctor
                            {
                                DoctorId = Convert.ToInt32(reader["doctor_id"]),
                                FirstName = reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                Specialization = reader["specialization"].ToString(),
                                DepartmentId = Convert.ToInt32(reader["department_id"]),
                                Email = reader["email"].ToString(),
                                PhoneNumber = reader["phone_number"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
