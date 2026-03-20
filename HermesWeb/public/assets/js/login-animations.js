// Animations specific for login page
class LoginAnimations {
    constructor() {
        this.init();
    }

    init() {
        this.setupStaggeredAnimations();
        this.setupInputInteractions();
        this.setupBackgroundEffects();
        this.setupPerformanceOptimizations();
    }

    setupStaggeredAnimations() {
        // Animate elements with staggered delay
        const animatedElements = document.querySelectorAll('.animated-input');
        
        animatedElements.forEach(element => {
            const delay = element.getAttribute('data-delay') || 0;
            
            setTimeout(() => {
                element.style.animationDelay = `${delay}ms`;
            }, 50);
        });

        // Logo special animation
        const logoIcon = document.querySelector('.logo-icon');
        const logoText = document.querySelector('.logo-text');
        
        if (logoIcon && logoText) {
            setTimeout(() => {
                logoIcon.style.animation = 'bounceIn 1s ease-out forwards';
                logoText.style.animation = 'textGlow 2s ease-in-out infinite alternate';
            }, 500);
        }
    }

    setupInputInteractions() {
        const formInputs = document.querySelectorAll('.form-input');
        
        formInputs.forEach(input => {
            // Real-time validation feedback
            input.addEventListener('input', (e) => {
                this.handleInputValidation(e.target);
            });
            
            // Focus effects
            input.addEventListener('focus', (e) => {
                this.enhanceFocusState(e.target);
            });
            
            input.addEventListener('blur', (e) => {
                this.removeFocusState(e.target);
            });
        });

        // Password strength indicator (optional enhancement)
        const passwordInput = document.getElementById('senha');
        if (passwordInput) {
            passwordInput.addEventListener('input', (e) => {
                this.showPasswordStrength(e.target.value);
            });
        }
    }

    setupBackgroundEffects() {
        // Mouse move parallax effect
        document.addEventListener('mousemove', (e) => {
            this.handleMouseMove(e);
        });

        // Touch move for mobile devices
        document.addEventListener('touchmove', (e) => {
            if (e.touches.length > 0) {
                this.handleMouseMove(e.touches[0]);
            }
        });
    }

    setupPerformanceOptimizations() {
        // Throttle expensive animations
        this.throttledMouseMove = this.throttle(this.handleMouseMove.bind(this), 16);
        
        // Use passive event listeners for better performance
        document.addEventListener('mousemove', this.throttledMouseMove, { passive: true });
        document.addEventListener('touchmove', this.throttledMouseMove, { passive: true });
    }

    handleInputValidation(input) {
        const isValid = input.checkValidity();
        const isEmpty = input.value.trim() === '';
        
        // Remove previous states
        input.classList.remove('success', 'error');
        
        if (!isEmpty) {
            if (isValid) {
                input.classList.add('success');
            } else {
                input.classList.add('error');
            }
        }
    }

    enhanceFocusState(input) {
        const formGroup = input.closest('.form-group');
        if (formGroup) {
            formGroup.style.transform = 'translateY(-2px)';
            formGroup.style.zIndex = '10';
        }
    }

    removeFocusState(input) {
        const formGroup = input.closest('.form-group');
        if (formGroup) {
            formGroup.style.transform = 'translateY(0)';
            formGroup.style.zIndex = '1';
        }
    }

    showPasswordStrength(password) {
        // Optional: Add password strength meter
        const strengthMeter = document.querySelector('.password-strength');
        if (!strengthMeter) return;

        const strength = this.calculatePasswordStrength(password);
        strengthMeter.className = `password-strength strength-${strength.level}`;
        strengthMeter.innerHTML = strength.message;
    }

    calculatePasswordStrength(password) {
        let strength = 0;
        let messages = [];

        if (password.length >= 8) strength++;
        else messages.push('Mínimo 8 caracteres');

        if (/[A-Z]/.test(password)) strength++;
        else messages.push('Inclua letras maiúsculas');

        if (/[0-9]/.test(password)) strength++;
        else messages.push('Inclua números');

        if (/[^A-Za-z0-9]/.test(password)) strength++;
        else messages.push('Inclua símbolos');

        const levels = [
            { level: 'weak', message: 'Senha fraca' },
            { level: 'medium', message: 'Senha média' },
            { level: 'strong', message: 'Senha forte' },
            { level: 'very-strong', message: 'Senha muito forte' }
        ];

        return {
            level: levels[Math.min(strength, 3)].level,
            message: messages.length > 0 ? messages.join(', ') : levels[Math.min(strength, 3)].message
        };
    }

    handleMouseMove(e) {
        const floatingElements = document.querySelectorAll('.floating-element');
        const mouseX = e.clientX / window.innerWidth;
        const mouseY = e.clientY / window.innerHeight;

        floatingElements.forEach((element, index) => {
            const speed = (index + 1) * 0.5;
            const x = (mouseX - 0.5) * speed * 20;
            const y = (mouseY - 0.5) * speed * 20;

            element.style.transform = `translate(${x}px, ${y}px)`;
        });
    }

    throttle(func, limit) {
        let inThrottle;
        return function(...args) {
            if (!inThrottle) {
                func.apply(this, args);
                inThrottle = true;
                setTimeout(() => inThrottle = false, limit);
            }
        };
    }

    // Page load animations
    animatePageLoad() {
        document.body.style.opacity = '0';
        
        setTimeout(() => {
            document.body.style.transition = 'opacity 0.5s ease-in';
            document.body.style.opacity = '1';
        }, 100);
    }
}

// Initialize animations when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    const loginAnimations = new LoginAnimations();
    loginAnimations.animatePageLoad();
});

// Add some utility functions for animations
const AnimationUtils = {
    // Smooth scroll to element
    scrollToElement(element, duration = 500) {
        const targetPosition = element.getBoundingClientRect().top + window.pageYOffset;
        const startPosition = window.pageYOffset;
        const distance = targetPosition - startPosition;
        let startTime = null;

        function animation(currentTime) {
            if (startTime === null) startTime = currentTime;
            const timeElapsed = currentTime - startTime;
            const run = easeInOutQuad(timeElapsed, startPosition, distance, duration);
            window.scrollTo(0, run);
            if (timeElapsed < duration) requestAnimationFrame(animation);
        }

        function easeInOutQuad(t, b, c, d) {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t + b;
            t--;
            return -c / 2 * (t * (t - 2) - 1) + b;
        }

        requestAnimationFrame(animation);
    },

    // Fade in elements sequentially
    fadeInElements(elements, delay = 100) {
        elements.forEach((element, index) => {
            setTimeout(() => {
                element.style.opacity = '0';
                element.style.transform = 'translateY(20px)';
                element.style.transition = 'all 0.5s ease-out';
                
                setTimeout(() => {
                    element.style.opacity = '1';
                    element.style.transform = 'translateY(0)';
                }, 50);
            }, index * delay);
        });
    }
};

// Export for potential use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { LoginAnimations, AnimationUtils };
}