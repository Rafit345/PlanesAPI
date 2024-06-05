using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ZeroZen.Models;

namespace ZeroZen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanesController : ControllerBase
    {
        private readonly string connectionString;
        public PlanesController(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
        }

        [HttpPost]
        public IActionResult CreatePlane(PlaneDto planeDTO) {
            try
            {
                using(var connection = new SqlConnection(connectionString)) {
                    connection.Open();

                    string sql = "INSERT INTO planes " + "(make, brand) VALUES " + "(@make, @brand)";

                    using(var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@make", planeDTO.Make);
                        command.Parameters.AddWithValue("@brand", planeDTO.Brand);

                        command.ExecuteNonQuery();
                    }

                }

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Plane", "Model Error");
                return BadRequest(ModelState);
            }


            return Ok();
        }

        [HttpGet]
        public IActionResult GetPlanes() {
            List<Plane> planes = new List<Plane>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM planes";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using(var reader = command.ExecuteReader()) { 
                            while (reader.Read()) { 
                                Plane plane = new Plane();

                                plane.Id = reader.GetInt32(0);
                                plane.Make = reader.GetString(1);   
                                plane.Brand = reader.GetString(2);  
                                plane.CreatedAt = reader.GetDateTime(3);

                                planes.Add(plane);

                            }
                        }

                    }

                }
            }
            catch(Exception ex) {
                ModelState.AddModelError("Plane", "Model Error");
                return BadRequest(ModelState);
            }

            return Ok (planes);
        }

        [HttpGet("{id}")]
        public IActionResult GetPlane(int id)
        {
            Plane plane = new Plane();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM planes WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id); 
                        using (var reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                plane.Id = reader.GetInt32(0);
                                plane.Make = reader.GetString(1);
                                plane.Brand = reader.GetString(2);
                                plane.CreatedAt = reader.GetDateTime(3);

                            }
                            else
                            {
                                return NotFound();
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Plane", "Model Error");
                return BadRequest(ModelState);
            }

            return Ok(plane);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePlane(int id, PlaneDto planeDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE planes SET make=@make, brand=@brand";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@make", planeDto.Make);
                            command.Parameters.AddWithValue("@brand", planeDto.Brand);

                            command.ExecuteNonQuery();
                        }


                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Plane", "Model Error");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePlane(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "DELETE planes WHERE id=@id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }


                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Plane", "Model Error");
                return BadRequest(ModelState);
            }

            return Ok();
        }

    }
}
