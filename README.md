# 🚗 Gestão Garagem

Este projeto é uma aplicação web completa voltada para o gerenciamento de garagens multimarcas, adotando uma identidade visual moderna e minimalista inspirada na plataforma **Webmotors** (tons de vermelho, cinza claro, branco e design responsivo baseado em cards).

---

## 🎓 Contexto Acadêmico

Este software foi desenvolvido como projeto prático de avaliação para a disciplina de **Técnicas de Programação I**, sob a orientação e supervisão do **[Prof. Me. Henrique Dezani](https://github.com/professordezani)**.

*   **Instituição:** FATEC (Faculdade de Tecnologia) de Olímpia, São Paulo.
*   **Curso:** Tecnologia em Desenvolvimento de Software Multiplataforma (DSM).

---

## 🛠️ Tecnologias e Recursos Utilizados

*   **Backend:** ASP.NET Core 8.0 / .NET MVC (C#)
*   **Banco de Dados:** SQLite (com Entity Framework Core para Migrations e persistência)
*   **Frontend:** HTML5, CSS3, JavaScript e Bootstrap 5.3
*   **Segurança:** Autenticação e Autorização baseadas em Cookies (RBAC - Controle de Acesso Baseado em Perfis)
*   **Manipulação de Mídia:** Upload de imagens e conversão automática para string binária **Base64** para armazenamento direto no banco local

---

## 👥 Perfis de Acesso e Regras de Negócio (Segurança)

O sistema conta com barreira estrita de segurança e controle de navegação em tempo de execução:

1.  **Administrador (`Admin`):** Acesso exclusivo à tela de *Gerenciar Usuários* para criação, visualização (foto 3x4) e exclusão de operadores do sistema. Não possui acesso às rotas comerciais.
2.  **Gerente (`Gerente`):** Visualização do Showroom, Estoque de Veículos, Histórico de Vendas e controle total sobre o painel de **Garantias**, com permissão exclusiva para estender prazos contratuais.
3.  **Vendedor (`Vendedor`):** Acesso restrito às rotas de Showroom (onde inicia vendas), Estoque e Histórico de Vendas. Não visualiza e não altera dados de garantias ou operadores.

---

## 🚀 Como Clonar e Rodar o Projeto

Siga os passos abaixo no terminal do seu ambiente de desenvolvimento local:

### 1. Clonar o Repositório
```bash
git clone [https://github.com/MrSautchuk/Projeto-Final-Tec_Prog_I.git](https://github.com/MrSautchuk/Projeto-Final-Tec_Prog_I.git)
cd Projeto-Final-Tec_Prog_I
