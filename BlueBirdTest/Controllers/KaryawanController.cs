using BlueBirdTest.Data;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BlueBirdTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KaryawanController : ControllerBase
    {
        private string _connection;
        public KaryawanController(IConfiguration configuration)
        {
            _connection = configuration["ConnectionStrings:DefaultConnection"];
        }
        [HttpGet("GetKaryawan")]
        public IActionResult GetKaryawan()
        {
            List<Karyawan> ret = new List<Karyawan>();
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();
                var query = "SELECT * FROM mst_karyawan";
                ret = connection.Query<Karyawan>(query).ToList();
                connection.Close();
            }
            return Ok(ret);
        }
        [HttpGet("GetKaryawanByNik")]
        public IActionResult GetKaryawanByNik(string nik)
        {
            Karyawan ret = new Karyawan();
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();
                var query = "SELECT * FROM mst_karyawan WHERE Nik=@Nik";
                ret = connection.Query<Karyawan>(query, new { Nik=nik}).FirstOrDefault();
                connection.Close();
            }
            return Ok(ret);
        }
        [HttpPost("CreateKaryawan")]
        public IActionResult CreateKaryawan([FromBody] Karyawan request)
        {
            string ret = "data gagal dimasukan";
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();
                var query = "SELECT * FROM mst_karyawan WHERE Nik=@Nik";
                int check = connection.Query<Karyawan>(query, new { Nik = request.Nik }).Count();
                connection.Close();
                if (check > 0)
                {
                    ret = "data sudah dimasukan";
                }
                else
                {
                    connection.Open();
                    var queryInsert = "INSERT INTO mst_karyawan (Nik,Nama,Alamat,NoTelpon,Status) VALUES (@Nik,@Nama,@Alamat,@NoTelpon,@Status)";
                    connection.Execute(queryInsert, request);
                    connection.Close();
                    ret = "data berhasil dimasukan";
                }
            }
            return Ok(ret);
        }
        [HttpPut("UpdateKaryawan")]
        public IActionResult UpdateKaryawan([FromBody] Karyawan request)
        {
            string ret = "data gagal dimasukan";
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();
                var query = "SELECT * FROM mst_karyawan WHERE Nik=@Nik";
                int check = connection.Query<Karyawan>(query, new { Nik = request.Nik }).Count();
                connection.Close();
                if (check <= 0)
                {
                    ret = "data tidak ditemukan";
                }
                else
                {
                    connection.Open();
                    var queryUpdate = "UPDATE mst_karyawan set Nama=@Nama, Alamat=@Alamat, NoTelpon=@NoTelpon, Status=@Status WHERE Nik=@Nik";
                    connection.Execute(queryUpdate, request);
                    connection.Close();
                    ret = "data berhasil diperbaharui";
                }
            }
            return Ok(ret);
        }
        [HttpDelete("DeleteKaryawan")]
        public IActionResult DeleteKaryawan(string nik)
        {
            string ret = "data gagal dimasukan";
            bool status = false;
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();
                var query = "SELECT * FROM mst_karyawan WHERE Nik=@Nik";
                int check = connection.Query<Karyawan>(query, new { Nik = nik }).Count();
                connection.Close();
                if (check <= 0)
                {
                    ret = "nik tidak ditemukan";
                }
                else
                {
                    connection.Open();
                    var queryDelete = "DELETE FROM mst_karyawan WHERE Nik=@Nik";
                    connection.Execute(queryDelete, new {Nik=nik});
                    connection.Close();
                    ret = "data berhasil di hapus";
                    status = true;
                }
            }
            return Ok(new {Message=ret,Status=status});
        }
    }
}
