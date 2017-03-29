using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparkTodo.DataAccess;
using SparkTodo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace SparkTodo.API.Controllers
{
    /// <summary>
    /// todo ����
    /// </summary>
    [Authorize]
    [Route("api/v1/[controller]")]
    public class CategoryController : Controller
    {
        readonly ICategoryRepository _categoryRepository;
        readonly ITodoItemRepository _todoItemRepository;

        /// <summary>
        /// CategoryController .ctor
        /// </summary>
        /// <param name="categoryRepository">categoryRepository</param>
        /// <param name="todoItemRepository">todoItemRepository</param>
        public CategoryController(ICategoryRepository categoryRepository, ITodoItemRepository todoItemRepository)
        {
            _categoryRepository = categoryRepository;
            _todoItemRepository = todoItemRepository;
        }

        /// <summary>
        /// ��ȡĳһ����������
        /// </summary>
        /// <param name="categoryId">����id</param>
        /// <returns></returns>
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> Get([FromQuery] int categoryId)
        {
            if(categoryId<=0)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            var list = await _todoItemRepository.SelectAsync(t => t.CategoryId == categoryId && t.IsDeleted == false,t=>t.CreatedTime);
            return Json(list);
        }

        /// <summary>
        /// �����û���ѯ todo �����б�
        /// </summary>
        /// <param name="userId">�û�id</param>
        /// <returns></returns>
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(int userId)
        {
            if (userId <= 0)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            List<Category> list =  await _categoryRepository.SelectAsync(ca => ca.UserId ==userId && !ca.IsDeleted,ca=>ca.CreatedTime,true);
            return Json(list);
        }

        /// <summary>
        /// �޸�todo������Ϣ
        /// </summary>
        /// <param name="category">������Ϣ</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Category category)
        {
            if(category.UserId <= 0 || category.CategoryId <=0 || String.IsNullOrEmpty(category.CategoryName))
            {
                return new StatusCodeResult(StatusCodes.Status406NotAcceptable);
            }
            category.UpdatedTime = DateTime.Now;
            var item = await _categoryRepository.UpdateAsync(category, "CategoryName", "UpdatedTime");
            return Json(item);
        }

        /// <summary>
        /// ���� todo ����
        /// </summary>
        /// <param name="category">������Ϣ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            if(category.UserId <= 0 || String.IsNullOrEmpty(category.CategoryName))
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            category.UpdatedTime = DateTime.Now;
            category.CreatedTime = DateTime.Now;
            category = await _categoryRepository.AddAsync(category);           
            return Json(category);
        }

        /// <summary>
        /// ɾ�� todo ����
        /// </summary>
        /// <param name="categoryId">����id</param>
        /// <returns></returns>
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            if(categoryId <= 0)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            var category = new Category(){ CategoryId = categoryId , IsDeleted = true };
            var result = await _categoryRepository.UpdateAsync(t=>t.CategoryId == categoryId,"IsDeleted");
            if(result)
            {
                return Ok();
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status501NotImplemented);
            }
        }
    }
}