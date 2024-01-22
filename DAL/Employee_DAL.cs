using ManagerWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection;
using ExcelDataReader;
using System.Data;

namespace ManagerWebApplication.DAL
{
    public class Employee_DAL
    {
        private readonly string? _connectionString;

        public Employee_DAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("conn");
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employeeList = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetEmployee]";

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.ID = reader["ID"].ToString();
                        employee.FirstName = reader["FirstName"].ToString();
                        employee.MiddleName = reader["MiddleName"].ToString();
                        employee.LastName = reader["LastName"].ToString();
                        employee.Gender = reader["Gender"].ToString();
                        DateTime tempDate;

                        DateTime? dateOfBirth = DateTime.TryParse(reader["DateOfBirth"].ToString(), out tempDate) ? tempDate : (DateTime?)null;
                        employee.DateOfBirth = dateOfBirth;
                        employee.PhoneNumber = reader["PhoneNumber"].ToString();
                        employee.Email = reader["Email"].ToString();
                        employee.LocalAddress = reader["LocalAddress"].ToString();
                        employee.PositionID = reader["PositionID"].ToString();
                        employee.PositionName = reader["PositionName"].ToString();
                        employee.DepartmentID = reader["DepartmentID"].ToString();
                        employee.DepartmentName = reader["DepartmentName"].ToString();
                        employee.Salary = reader["Salary"].ToString();
                        DateTime? startDate = DateTime.TryParse(reader["StartDate"].ToString(), out tempDate) ? tempDate : (DateTime?)null;
                        employee.StartDate = startDate;
                        DateTime? endDate = DateTime.TryParse(reader["EndDate"].ToString(), out tempDate) ? tempDate : (DateTime?)null;
                        employee.EndDate = endDate;
                        employee.AvaName = reader["AvaName"].ToString();
                        employeeList.Add(employee);
                    }
                }
            }

            return employeeList;
        }

        public Employee? GetEmployeeByID(string id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetByID]";
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.ID = reader["ID"].ToString();
                        employee.FirstName = reader["FirstName"].ToString();
                        employee.MiddleName = reader["MiddleName"].ToString();
                        employee.LastName = reader["LastName"].ToString();
                        employee.Gender = reader["Gender"].ToString();
                        DateTime tempDate;

                        DateTime? dateOfBirth = DateTime.TryParse(reader["DateOfBirth"].ToString(), out tempDate) ? tempDate : (DateTime?)null;
                        employee.DateOfBirth = dateOfBirth;
                        employee.PhoneNumber = reader["PhoneNumber"].ToString();
                        employee.Email = reader["Email"].ToString();
                        employee.LocalAddress = reader["LocalAddress"].ToString();
                        employee.PositionID = reader["PositionID"].ToString();
                        employee.PositionName = reader["PositionName"].ToString();
                        employee.DepartmentID = reader["DepartmentID"].ToString();
                        employee.DepartmentName = reader["DepartmentName"].ToString();
                        employee.Salary = reader["Salary"].ToString();
                        DateTime? startDate = DateTime.TryParse(reader["StartDate"].ToString(), out tempDate) ? tempDate : (DateTime?)null;
                        employee.StartDate = startDate;
                        DateTime? endDate = DateTime.TryParse(reader["EndDate"].ToString(), out tempDate) ? tempDate : (DateTime?)null;
                        employee.EndDate = endDate;
                        employee.AvaName = reader["AvaName"].ToString();

                        return employee;
                    }
                }
            }

            return null;
        }

        public void DeleteEmployee(string ID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[DeleteEmployee]";
                    command.Parameters.AddWithValue("@ID", ID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void CreateEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[CreateEmployee]";

                    command.Parameters.AddWithValue("@ID", employee.ID);
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", employee.MiddleName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@Gender", employee.Gender ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", employee.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LocalAddress", employee.LocalAddress ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PositionID", employee.PositionID);
                    command.Parameters.AddWithValue("@Salary", employee.Salary ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@StartDate", employee.StartDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", employee.EndDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AvaName", employee.AvaName ?? (object)DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[UpdateEmployee]";
                    command.Parameters.AddWithValue("@ID", employee.ID);
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", employee.MiddleName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@Gender", employee.Gender ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", employee.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LocalAddress", employee.LocalAddress ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PositionID", employee.PositionID);
                    command.Parameters.AddWithValue("@Salary", employee.Salary ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@StartDate", employee.StartDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", employee.EndDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AvaName", employee.AvaName ?? (object)DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public List<Employee> ReadEmployeeFile(string filePath)
        {
            List<Employee> employeeList = new List<Employee>();
            string fileExtension = Path.GetExtension(filePath).ToLowerInvariant(); // Case insensitive
            try
            {
                if (fileExtension == ".csv")
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        string? line = reader.ReadLine();
                        if (line == null) return employeeList;

                        string[] headers = line.Split(',');
                        PropertyInfo[] properties = typeof(Employee).GetProperties();

                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            if (string.IsNullOrWhiteSpace(line)) continue;
                            //if (line == null) continue;

                            var values = line.Split(',');
                            if (values.All(string.IsNullOrWhiteSpace)) continue;
                            Employee employee = new Employee();
                            for (int i = 0; i < headers.Length; i++)
                            {
                                string? headerName = i < headers.Length ? headers[i].Trim() : null;
                                if (string.IsNullOrWhiteSpace(headerName)) continue;

                                PropertyInfo? property = properties.FirstOrDefault(p => p.Name.Equals(headers[i].Trim(), StringComparison.InvariantCultureIgnoreCase));
                                if (property != null && i < values.Length && !string.IsNullOrWhiteSpace(values[i]))
                                {
                                    object? value = null;

                                    if (property.PropertyType == typeof(DateTime?))
                                    {
                                        DateTime dateValue;
                                        if (DateTime.TryParse(values[i], out dateValue))
                                        {
                                            value = dateValue;
                                        }
                                        else
                                        {
                                            value = null;
                                        }
                                    }
                                    else
                                    {
                                        // For other property types, use Convert.ChangeType.
                                        value = Convert.ChangeType(values[i], property.PropertyType);
                                    }
                                    property.SetValue(employee, value);
                                }
                            }
                            employeeList.Add(employee);
                        }
                    }
                }
                else if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var headers = reader.Read() ? Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetString(i)).ToArray() : null;
                            if (headers == null) return employeeList;

                            Dictionary<string, PropertyInfo> headertoPropertyMap = new Dictionary<string, PropertyInfo>();
                            PropertyInfo[] properties = typeof(Employee).GetProperties();
                            foreach (var property in properties)
                            {
                                string? headerName = headers.FirstOrDefault(h => h != null && h.Trim().Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));
                                if (headerName != null)
                                {
                                    headertoPropertyMap.Add(headerName, property);
                                }
                            }

                            while (reader.Read())
                            {
                                if (Enumerable.Range(0, reader.FieldCount).All(i => reader.IsDBNull(i))) continue; // Skip empty rows (all columns are null)
                                Employee employee = new Employee();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string? headerName = i < headers.Length && headers[i] != null ? headers[i].Trim() : null;
                                    if (string.IsNullOrWhiteSpace(headerName)) continue;
                                    //PropertyInfo? property = properties.FirstOrDefault(p => p.Name.Equals(headerName, StringComparison.InvariantCultureIgnoreCase));
                                    PropertyInfo? property = headertoPropertyMap.GetValueOrDefault(headerName);
                                    if (property != null && !reader.IsDBNull(i))
                                    {
                                        object? value = null;

                                        if (property.PropertyType == typeof(DateTime?))
                                        {
                                            DateTime dateValue;
                                            if (DateTime.TryParse(reader.GetValue(i).ToString().AsSpan(), out dateValue))
                                            {
                                                value = dateValue;
                                            }
                                            else
                                            {
                                                value = null;
                                            }
                                        }
                                        else
                                        {
                                            // For other property types, use Convert.ChangeType.
                                            value = Convert.ChangeType(reader.GetValue(i), property.PropertyType);
                                        }
                                        
                                        property.SetValue(employee, value);
                                    }
                                }
                                employeeList.Add(employee);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return employeeList;
        }

        public void InsertEmployees(List<Employee> employees)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var employee in employees)
                        {
                            if (!string.IsNullOrEmpty(employee.AvaName))
                            {
                                employee.AvaName = "/hr_images/avatar/" + employee.AvaName;
                            }
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandType = System.Data.CommandType.StoredProcedure;
                                command.CommandText = "[dbo].[CreateEmployee]";

                                command.Parameters.AddWithValue("@ID", employee.ID);
                                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                                command.Parameters.AddWithValue("@MiddleName", employee.MiddleName ?? (object)DBNull.Value);
                                command.Parameters.AddWithValue("@LastName", employee.LastName);
                                command.Parameters.AddWithValue("@Gender", employee.Gender ?? (object)DBNull.Value);
                                command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth ?? (object)DBNull.Value);
                                command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber ?? (object)DBNull.Value);
                                command.Parameters.AddWithValue("@Email", employee.Email ?? (object)DBNull.Value);
                                command.Parameters.AddWithValue("@LocalAddress", employee.LocalAddress ?? (object)DBNull.Value);
                                command.Parameters.AddWithValue("@PositionID", employee.PositionID);
                                command.Parameters.AddWithValue("@Salary", employee.Salary ?? (object)DBNull.Value);
                                command.Parameters.AddWithValue("@StartDate", employee.StartDate ?? (object)DBNull.Value);
                                command.Parameters.AddWithValue("@EndDate", employee.EndDate ?? (object)DBNull.Value);
                                command.Parameters.AddWithValue("@AvaName", employee.AvaName ?? (object)DBNull.Value);

                                command.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                connection.Close();
            }
        }

        public List<Employee> GetAllDepartments()
        {
            List<Employee> departmentList = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetDepartment]";

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee department = new Employee();
                        department.DepartmentID = reader["DepartmentID"].ToString();
                        department.DepartmentName = reader["DepartmentName"].ToString();

                        departmentList.Add(department);
                    }
                }
            }
            return departmentList;
        }

        public List<Employee> GetAllPositions()
        {
            List<Employee> positionList = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetPosition]";

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee position = new Employee();
                        position.PositionID = reader["PositionID"].ToString();
                        position.PositionName = reader["PositionName"].ToString();
                        position.DepartmentID = reader["DepartmentID"].ToString();

                        positionList.Add(position);
                    }
                }
            }
            return positionList;
        }

        public List<Employee> GetPositionByDepartment(string departmentID)
        {
            List<Employee> positionByDepartment = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetPositionbyDepartment]";
                    command.Parameters.AddWithValue("@departmentID", departmentID);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee position = new Employee();
                        position.PositionID = reader["PositionID"].ToString();
                        position.PositionName = reader["PositionName"].ToString();

                        positionByDepartment.Add(position);
                    }
                }
            }
            return positionByDepartment;
        }
    }
}