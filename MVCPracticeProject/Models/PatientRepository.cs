using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCPracticeProject.Models
{
    public class PatientRepository
    {
        public string connection = "Data Source=.;Initial Catalog=AssignmentDB;User ID=sa;Password=mcc#1234;";

        // READ ALL
        public List<Patient> GetPatients()
        {
            List<Patient> patients = new List<Patient>();

            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM labTest.PatientTbl", con))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            patients.Add(new Patient()
                            {
                                PatientId = Convert.ToInt32(dr["patient_id"]),
                                FirstName = dr["first_name"].ToString(),
                                LastName = dr["last_name"].ToString(),
                                Gender = dr["gender"].ToString(),
                                Age = Convert.ToInt32(dr["age"]),
                                BloodGroup = dr["blood_group"].ToString(),
                                DoctorId = Convert.ToInt32(dr["doctor_id"]),
                                Disease = dr["disease"].ToString(),
                                AdmissionDate = Convert.ToDateTime(dr["admission_date"]),
                                Email = dr["email"].ToString(),
                                PhoneNumber = dr["phone_number"].ToString()
                            });
                        }
                    }
                }
            }

            return patients;
        }

        // READ BY ID
        public Patient GetPatientById(int id)
        {
            Patient patient = null;

            using (SqlConnection con = new SqlConnection(connection))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM labTest.PatientTbl WHERE patient_id = @id", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            patient = new Patient()
                            {
                                PatientId = Convert.ToInt32(dr["patient_id"]),
                                FirstName = dr["first_name"].ToString(),
                                LastName = dr["last_name"].ToString(),
                                Gender = dr["gender"].ToString(),
                                Age = Convert.ToInt32(dr["age"]),
                                BloodGroup = dr["blood_group"].ToString(),
                                DoctorId = Convert.ToInt32(dr["doctor_id"]),
                                Disease = dr["disease"].ToString(),
                                AdmissionDate = Convert.ToDateTime(dr["admission_date"]),
                                Email = dr["email"].ToString(),
                                PhoneNumber = dr["phone_number"].ToString()
                            };
                        }
                    }
                }
            }

            return patient;
        }

        // INSERT / CREATE
        public string InsertPatient(Patient patient)
        {
            if (patient == null)
            {
                return "Patient object is required.";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    string query = @"INSERT INTO labTest.PatientTbl
                                    (
                                        first_name,
                                        last_name,
                                        gender,
                                        age,
                                        blood_group,
                                        doctor_id,
                                        disease,
                                        admission_date,
                                        email,
                                        phone_number
                                    )
                                    VALUES
                                    (
                                        @FirstName,
                                        @LastName,
                                        @Gender,
                                        @Age,
                                        @BloodGroup,
                                        @DoctorId,
                                        @Disease,
                                        @AdmissionDate,
                                        @Email,
                                        @PhoneNumber
                                    )";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", (object)patient.FirstName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@LastName", (object)patient.LastName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Gender", (object)patient.Gender ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Age", patient.Age);
                        cmd.Parameters.AddWithValue("@BloodGroup", (object)patient.BloodGroup ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DoctorId", patient.DoctorId);
                        cmd.Parameters.AddWithValue("@Disease", (object)patient.Disease ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@AdmissionDate", patient.AdmissionDate == DateTime.MinValue ? DateTime.Now : patient.AdmissionDate);
                        cmd.Parameters.AddWithValue("@Email", (object)patient.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PhoneNumber", (object)patient.PhoneNumber ?? DBNull.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return "Patient created successfully.";
                        }

                        return "Patient could not be created.";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CreatePatient(Patient patient)
        {
            return InsertPatient(patient);
        }

        // UPDATE
        public string UpdatePatient(Patient patient)
        {
            if (patient == null)
            {
                return "Patient object is required.";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    string query = @"UPDATE labTest.PatientTbl
                                    SET
                                        first_name = @FirstName,
                                        last_name = @LastName,
                                        gender = @Gender,
                                        age = @Age,
                                        blood_group = @BloodGroup,
                                        doctor_id = @DoctorId,
                                        disease = @Disease,
                                        admission_date = @AdmissionDate,
                                        email = @Email,
                                        phone_number = @PhoneNumber
                                    WHERE patient_id = @PatientId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@PatientId", patient.PatientId);
                        cmd.Parameters.AddWithValue("@FirstName", (object)patient.FirstName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@LastName", (object)patient.LastName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Gender", (object)patient.Gender ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Age", patient.Age);
                        cmd.Parameters.AddWithValue("@BloodGroup", (object)patient.BloodGroup ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DoctorId", patient.DoctorId);
                        cmd.Parameters.AddWithValue("@Disease", (object)patient.Disease ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@AdmissionDate", patient.AdmissionDate == DateTime.MinValue ? DateTime.Now : patient.AdmissionDate);
                        cmd.Parameters.AddWithValue("@Email", (object)patient.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PhoneNumber", (object)patient.PhoneNumber ?? DBNull.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return "Patient updated successfully.";
                        }

                        return "Patient not found or could not be updated.";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // DELETE
        public string DeletePatient(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM labTest.PatientTbl WHERE patient_id = @id", con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@id", id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return $"Patient with id {id} was deleted successfully.";
                        }

                        return $"Failed to delete patient with id {id}.";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeletePatientById(int id)
        {
            return DeletePatient(id);
        }
    }
}
