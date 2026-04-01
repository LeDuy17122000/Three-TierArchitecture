using Business.DTOs;
using Business.Entities;
using DataAccess.Data;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Repositories
{
    public class EmployeeRepo : IEmployee // Giả sử bạn có Interface này
    {
        private readonly AppDbContext appDbContext;

        public EmployeeRepo(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<ServiceResponse> AddAsync(Employee employee)
        {
            // 1. Kiểm tra xem nhân viên đã tồn tại chưa (không phân biệt hoa thường)
            var check = await appDbContext.Employees
                .FirstOrDefaultAsync(_ => _.Name.ToLower() == employee.Name.ToLower());

            if (check != null)
            {
                return new ServiceResponse(false, "User already exist");
            }

            // 2. Thêm nhân viên mới
            appDbContext.Employees.Add(employee);

            // 3. Lưu thay đổi vào Database
            await appDbContext.SaveChangesAsync();

            return new ServiceResponse(true, "Added");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            // 1. Tìm nhân viên theo ID
            var employee = await appDbContext.Employees.FindAsync(id);

            // 2. Nếu không tìm thấy, trả về thông báo lỗi
            if (employee == null)
            {
                return new ServiceResponse(false, "User not found");
            }

            // 3. Xóa nhân viên khỏi bộ nhớ context
            appDbContext.Employees.Remove(employee);

            // 4. Lưu thay đổi xuống Database (Lưu ý: dùng appDbContext.SaveChangesAsync)
            await appDbContext.SaveChangesAsync();

            // 5. Trả về thông báo thành công
            return new ServiceResponse(true, "Deleted");
        }
        // 1. Lấy danh sách tất cả nhân viên
        public async Task<List<Employee>> GetAsync() =>
            await appDbContext.Employees.AsNoTracking().ToListAsync();

        // 2. Tìm nhân viên theo ID
        public async Task<Employee> GetByIdAsync(int id) =>
            await appDbContext.Employees.FindAsync(id);

        // 3. Cập nhật thông tin nhân viên
        public async Task<ServiceResponse> UpdateAsync(Employee employee)
        {
            // Đánh dấu thực thể là đã thay đổi
            appDbContext.Update(employee);

            // Lưu thay đổi (nhớ dùng appDbContext.SaveChangesAsync)
            await appDbContext.SaveChangesAsync();

            return new ServiceResponse(true, "Updated");
        }
        private async Task SaveChangesAsync() => await appDbContext.SaveChangesAsync();
    }
}
