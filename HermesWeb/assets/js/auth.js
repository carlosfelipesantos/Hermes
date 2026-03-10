// Validações e interações para páginas de autenticação
class AuthForms {
    constructor() {
        this.init();
    }

    init() {
        this.setupFormValidation();
        this.setupPasswordToggle();
        this.setupAnimations();
        this.setupRadioOptions();
    }

    setupFormValidation() {
        const forms = document.querySelectorAll('form');
        
        forms.forEach(form => {
            form.addEventListener('submit', (e) => {
                const inputs = form.querySelectorAll('input[required]');
                let isValid = true;

                inputs.forEach(input => {
                    if (!input.value.trim()) {
                        this.showError(input, 'Este campo é obrigatório');
                        isValid = false;
                    } else {
                        this.clearError(input);
                    }

                    // Validação específica de email
                    if (input.type === 'email' && input.value.trim()) {
                        if (!this.isValidEmail(input.value)) {
                            this.showError(input, 'Por favor, insira um e-mail válido');
                            isValid = false;
                        }
                    }
                });

                if (!isValid) {
                    e.preventDefault();
                } else {
                    // Mostrar loading no botão
                    const submitBtn = form.querySelector('button[type="submit"]');
                    if (submitBtn) {
                        this.showLoading(submitBtn);
                    }
                }
            });
        });
    }

    setupPasswordToggle() {
        const toggleButtons = document.querySelectorAll('.password-toggle');
        
        toggleButtons.forEach(button => {
            button.addEventListener('click', () => {
                const input = button.parentElement.querySelector('.form-input');
                const icon = button.querySelector('i');
                
                if (input.type === 'password') {
                    input.type = 'text';
                    icon.classList.remove('fa-eye');
                    icon.classList.add('fa-eye-slash');
                    button.setAttribute('aria-label', 'Ocultar senha');
                } else {
                    input.type = 'password';
                    icon.classList.remove('fa-eye-slash');
                    icon.classList.add('fa-eye');
                    button.setAttribute('aria-label', 'Mostrar senha');
                }
            });
        });
    }

    setupRadioOptions() {
        const radioOptions = document.querySelectorAll('.radio-option');
        
        radioOptions.forEach(option => {
            option.addEventListener('click', () => {
                const radioInput = option.querySelector('input[type="radio"]');
                radioInput.checked = true;
                
                // Remover seleção de outras opções
                radioOptions.forEach(otherOption => {
                    if (otherOption !== option) {
                        otherOption.classList.remove('selected');
                    }
                });
                
                option.classList.add('selected');
            });
            
            // Teclado accessibility
            option.addEventListener('keypress', (e) => {
                if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault();
                    option.click();
                }
            });
        });
    }

    setupAnimations() {
        // Animações de entrada para elementos do formulário
        const elements = document.querySelectorAll('.form-group, .btn-login, .btn-register, .register-options');
        
        elements.forEach((element, index) => {
            element.style.opacity = '0';
            element.style.transform = 'translateY(20px)';
            
            setTimeout(() => {
                element.style.transition = 'all 0.5s ease-out';
                element.style.opacity = '1';
                element.style.transform = 'translateY(0)';
            }, index * 100);
        });
    }

    isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    showError(input, message) {
        this.clearError(input);
        
        input.style.borderColor = 'var(--error)';
        
        const errorDiv = document.createElement('div');
        errorDiv.className = 'error-message';
        errorDiv.style.color = 'var(--error)';
        errorDiv.style.fontSize = '0.8rem';
        errorDiv.style.marginTop = '0.25rem';
        errorDiv.textContent = message;
        
        input.parentNode.appendChild(errorDiv);
    }

    clearError(input) {
        input.style.borderColor = '';
        
        const existingError = input.parentNode.querySelector('.error-message');
        if (existingError) {
            existingError.remove();
        }
    }

    showLoading(button) {
        button.classList.add('loading');
        button.disabled = true;
        
        // Reverter após 3 segundos (fallback)
        setTimeout(() => {
            button.classList.remove('loading');
            button.disabled = false;
        }, 3000);
    }
}

// Função global para redirecionamento de cadastro
function redirecionarCadastro() {
    const tipoCadastro = document.querySelector('input[name="tipoCadastro"]:checked');
    if (!tipoCadastro) {
        alert('Por favor, selecione um tipo de cadastro');
        return;
    }
    
    if (tipoCadastro.value === 'cliente') {
        window.location.href = '../cadastro/cliente.jsp';
    } else if (tipoCadastro.value === 'transportador') {
        window.location.href = '../cadastro/transportador.jsp';
    }
}

// Inicializar quando o DOM estiver carregado
document.addEventListener('DOMContentLoaded', () => {
    new AuthForms();
    
    // Adicionar evento de Enter nos radio buttons
    document.querySelectorAll('.radio-option').forEach(option => {
        option.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                this.querySelector('input[type="radio"]').checked = true;
                redirecionarCadastro();
            }
        });
    });
});