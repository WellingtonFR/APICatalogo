using ApiCatalogo.Controllers;
using ApiCatalogo.Repository;
using APICatalogo.Context;
using APICatalogo.DTO;
using APICatalogo.DTO.Mapping;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace APICatalogoXUnitTests
{
    public class CategoriasUnitTestController
    {

        private IMapper mapper;
        private UnitOfWork repository;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=127.0.0.1,Port=3306;Database=Catalogo;Uid=root;Pwd=#Sudo#4321*";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseMySql(connectionString).Options;
        }

        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            //DBUnitTestsMockInitializer db = new DBUnitTestsMockInitializer();
            //db.Seed(context);

            repository = new UnitOfWork(context);
        }

        //testes unitários================================================
        // testar o método GET
        //====================================Get(int id) =====================================
        [Fact]
        public void GetCategoriaById_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 4;

            //Act  
            var data = controller.Get(catId);
            Console.WriteLine(data);

            //Assert  
            Assert.IsType<CategoriaDTO>(data.Value);
        }

        [Fact]
        public void GetCategoriaById_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 9999;

            //Act  
            var data = controller.Get(catId);

            //Assert  
            Assert.IsType<NotFoundResult>(data.Result);
        }
        [Fact]
        public void GetCategoriaById_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            int? catId = null;

            //Act  
            var data = controller.Get((int)catId);

            //Assert  
            Assert.IsType<BadRequestResult>(data.Result);
        }

        [Fact]
        public void GetCategoriaById_MatchResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            int? catId = 1;

            //Act  
            var data = controller.Get((int)catId);

            //Assert  
            Assert.IsType<CategoriaDTO>(data.Value);
            var cat = data.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;

            Assert.Equal("Bebidas alterada", cat.Nome);
            Assert.Equal("bebidas21.jpg", cat.ImagemUrl);
        }

        //===============================================Get=====================================
        [Fact]
        public void GetCategorias_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);

            //Act  
            var data = controller.Get();

            //Assert  
            Assert.IsType<List<CategoriaDTO>>(data.Value);
        }

        [Fact]
        public void GetCategorias_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);

            //Act  
            var data = controller.Get();
            //data = null;

            //if (data != null)
            //Assert  
            Assert.IsType<BadRequestResult>(data.Result);
        }

        [Fact]
        public void GetCategorias_MatchResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);

            //Act  
            var data = controller.Get();

            //Assert  
            Assert.IsType<List<CategoriaDTO>>(data.Value);
            var cat = data.Value.Should().BeAssignableTo<List<CategoriaDTO>>().Subject;

            Assert.Equal("Bebidas alterada", cat[0].Nome);
            Assert.Equal("bebidas21.jpg", cat[0].ImagemUrl);

            Assert.Equal("Lanches", cat[1].Nome);
            Assert.Equal("lanches.jpg", cat[1].ImagemUrl);
        }

        //====================================Post=====================================

        [Fact]
        public void Post_Categoria_AddValidData_Return_CreatedResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);

            var cat = new CategoriaDTO() { Nome = "Teste Unitario 1", ImagemUrl = "testecat.jpg" };

            //Act  
            var data = controller.Post(cat);

            //Assert  
            Assert.IsType<CreatedAtRouteResult>(data);
        }

        [Fact]
        public void Post_Categoria_Add_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);

            var cat = new CategoriaDTO()
            {
                Nome = "Teste Unitario dados invalidos de nome muito " +
                "loooooooooooooooooonnnnnnnnnnngggggggggggggggggggoooooooooooooo"
                ,
                ImagemUrl = "testecat.jpg"
            };

            //Act              
            var data = controller.Post(cat);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public void Post_Categoria_Add_ValidData_MatchResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);

            var cat = new CategoriaDTO() { Nome = "Teste Unitario 1", ImagemUrl = "testecat.jpg" };

            //Act  
            var data = controller.Post(cat);

            //Assert  
            Assert.IsType<CreatedAtRouteResult>(data);

            var okResult = data.Should().BeOfType<CreatedAtRouteResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;

            Assert.Equal(3, okResult.Value);
        }

        //===========================================Put =====================================

        [Fact]
        public void Put_Categoria_Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 2;

            //Act  
            var existingPost = controller.Get(catId);
            //var okResult = existingPost.Should().BeOfType<CategoriaDTO>().Subject;
            var result = existingPost.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;
            //var result = okResult.Should().BeAssignableTo<CategoriaDTO>().Subject;

            var catDto = new CategoriaDTO();
            catDto.CategoriaId = catId;
            catDto.Nome = "Categoria Atualizada - Testes 1";
            catDto.ImagemUrl = result.ImagemUrl;

            var updatedData = controller.Put(catId, catDto);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public void Put_Categoria_Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 1000;

            //Act  
            var existingPost = controller.Get(catId);
            //var okResult = existingPost.Should().BeOfType<CategoriaDTO>().Subject;
            var result = existingPost.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;
            //var result = okResult.Should().BeAssignableTo<CategoriaDTO>().Subject;

            var catDto = new CategoriaDTO();
            catDto.CategoriaId = result.CategoriaId;
            catDto.Nome = "Categoria Atualizada - Testes 1 com nome muiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiitttttttttttttttttttttttttttttttttooooooooooooooo looooooooooooooooooooooooooooooooooooooooooooooonnnnnnnnnnnnnnnnnnnnnnnnnnnngo";
            catDto.ImagemUrl = result.ImagemUrl;

            var data = controller.Put(catId, catDto);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }
        //=======================================Delete ===================================
        [Fact]
        public void Delete_Categoria_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 14;

            //Act  
            var data = controller.Delete(catId);

            //Assert  
            Assert.IsType<CategoriaDTO>(data.Value);
        }

        [Fact]
        public void Delete_Categoria_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 999999;

            //Act  
            var data = controller.Delete(catId);

            //Assert  
            Assert.IsType<NotFoundResult>(data.Result);
        }

        [Fact]
        public void Delete_Categoria_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            int? catId = null;

            //Act  
            var data = controller.Delete((int)catId);

            //Assert  
            Assert.IsType<BadRequestResult>(data.Result);
        }
    }
}