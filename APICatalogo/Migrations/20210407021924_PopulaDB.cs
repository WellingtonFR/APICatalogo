using Microsoft.EntityFrameworkCore.Migrations;

namespace APICatalogo.Migrations
{
    public partial class PopulaDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Categorias(Nome,ImagemUrl) values('bebidas',"+"'https://www.supermuffato.com.br/logo.png')");
            migrationBuilder.Sql("Insert into Categorias(Nome,ImagemUrl) values('Lanches',"+"'https://www.supermuffato.com.br/logo.png')");
            migrationBuilder.Sql("Insert into Categorias(Nome,ImagemUrl) values('Sobremesas',"+"'https://www.supermuffato.com.br/logo.png')");
            
            migrationBuilder.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,"
                + "DataCadastro,CategoriaId) Values('Coca Cola','Refrigerante de cola 350ml',"
                + "5.45,'https://www.supermuffato.com.br/coca.png',50,now()," +
                "(Select CategoriaId from Categorias where Nome='Bebidas')" +
                ")");
            migrationBuilder.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,"
                + "DataCadastro,CategoriaId) Values('Coca Cola','Refrigerante de cola 350ml',"
                + "5.45,'https://www.supermuffato.com.br/coca.png',50,now()," +
                "(Select CategoriaId from Categorias where Nome='Lanches')" +
                ")");
            migrationBuilder.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,"
                + "DataCadastro,CategoriaId) Values('Coca Cola','Refrigerante de cola 350ml',"
                + "5.45,'https://www.supermuffato.com.br/coca.png',50,now()," +
                "(Select CategoriaId from Categorias where Nome='Sobremesas')" +
                ")");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categorias");
            migrationBuilder.Sql("Delete from Produtos");
        }
    }
}
