// Você pode passar a mensagem pela URL, por exemplo:
// erro.html?mensagem=Falha%20ao%20carregar%20os%20dados

const params = new URLSearchParams(window.location.search);
const mensagem = params.get("mensagem");

const mensagemErro = document.getElementById("mensagemErro");

if (mensagemErro && mensagem) {
  mensagemErro.textContent = mensagem;
}