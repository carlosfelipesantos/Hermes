function toggleTransportador(ativo) {
    const bloco = document.getElementById('camposTransportador');
    if (!bloco) return;

    if (ativo) {
        bloco.classList.add('show');
    } else {
        bloco.classList.remove('show');
    }
}

// Inicializa comportamento ao carregar
document.addEventListener('DOMContentLoaded', function() {
    const radioCliente = document.querySelector('input[name="tipoUsuario"][value="cliente"]');
    const radioTransportador = document.querySelector('input[name="tipoUsuario"][value="transportador"]');

    if (radioCliente) {
        radioCliente.addEventListener('change', () => toggleTransportador(false));
    }
    if (radioTransportador) {
        radioTransportador.addEventListener('change', () => toggleTransportador(true));
    }

    // Reexibe automaticamente se transportador estava marcado
    if (radioTransportador && radioTransportador.checked) {
        toggleTransportador(true);
    }
});

/* === Máscara de Documento (CPF/CNPJ) === */
function mascararDocumento(input) {
    let valor = input.value.replace(/\D/g, ""); // remove tudo que não é número

    if (valor.length <= 11) {
        // CPF -> 000.000.000-00
        valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
        valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
        valor = valor.replace(/(\d{3})(\d{1,2})$/, "$1-$2");
    } else {
        // CNPJ -> 00.000.000/0000-00
        valor = valor.replace(/^(\d{2})(\d)/, "$1.$2");
        valor = valor.replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3");
        valor = valor.replace(/\.(\d{3})(\d)/, ".$1/$2");
        valor = valor.replace(/(\d{4})(\d)/, "$1-$2");
    }

    input.value = valor;
}

/* === Máscara de Telefone === */
function mascararTelefone(input) {
    let valor = input.value.replace(/\D/g, "");

    if (valor.length > 10) {
        // Formato (XX) XXXXX-XXXX
        valor = valor.replace(/^(\d{2})(\d{5})(\d{4}).*/, "($1) $2-$3");
    } else {
        // Formato (XX) XXXX-XXXX
        valor = valor.replace(/^(\d{2})(\d{4})(\d{0,4}).*/, "($1) $2-$3");
    }

    input.value = valor;
}

/* === Antes de enviar o formulário, limpar as máscaras === */
document.addEventListener("DOMContentLoaded", () => {
    const form = document.querySelector(".login-form");
    form.addEventListener("submit", () => {
        const docInput = document.getElementById("documento");
        const telInput = document.getElementById("telefone");

        if (docInput) {
            docInput.value = docInput.value.replace(/\D/g, "");
        }
        if (telInput) {
            telInput.value = telInput.value.replace(/\D/g, "");
        }
    });
});