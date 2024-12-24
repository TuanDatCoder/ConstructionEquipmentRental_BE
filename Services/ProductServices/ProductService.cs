using AutoMapper;
using BuildLease.Data.DTOs.Product;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductServices
{
    public class ProductService//:IProductService
    {

        private readonly IMapper _mapper;
        //private readonly IDecodeTokenHandler _decodeToken;

        //public async Task<List<ProductResponseDTO>> GetCourses(string? token, int? page, int? size, string? sortBy)
        //{

        //    List<Product> courses = new List<Product>();

        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        var decode = _decodeToken.decode(token);

        //        var currentCustomer = await _customerRepository.GetCustomerByUsername(decode.username);

        //        if (currentCustomer != null)
        //        {
        //            var customerCourse = await _customerCourseRepository.GetCustomerCoursesByCustomerId(currentCustomer.CustomerId);

        //            courses = await _courseRepository.GetCourses(page, size);

        //            courses = sortCourse(courses, sortBy);

        //            return _mapper.Map<List<CourseViewListResModel>>(courses.Where(x => x.Status.Equals(StatusEnums.Available.ToString())
        //            && !customerCourse.Select(uc => uc.CourseId).ToList().Contains(x.CourseId)).ToList());
        //        }
        //        else
        //        {
        //            courses = await _courseRepository.GetCourses(page, size);

        //            courses = sortCourse(courses, sortBy);

        //            return _mapper.Map<List<CourseViewListResModel>>(courses.Where(x => x.Status.Equals(StatusEnums.Available.ToString())).ToList());
        //        }
        //    }
        //    else
        //    {
        //        courses = await _courseRepository.GetCourses(page, size);

        //        courses = sortCourse(courses, sortBy);

        //        return _mapper.Map<List<CourseViewListResModel>>(courses.Where(x => x.Status.Equals(StatusEnums.Available.ToString())).ToList());
        //    }
        //}

    }
}
