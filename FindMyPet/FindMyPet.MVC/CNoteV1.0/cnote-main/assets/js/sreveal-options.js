
// Scroll Reveal Options

window.sr = ScrollReveal();
sr.reveal('.sr-element', {
    duration: 1000,
    scale: 1,
    origin: 'bottom',
    distance: '200px',
    viewOffset: { bottom: 100 }
});
sr.reveal('.sr-element.sr-duration-500', {duration: 500})
sr.reveal('.sr-element.sr-duration-750', {duration: 750})
sr.reveal('.sr-element.sr-duration-1250', {duration: 1250})
sr.reveal('.sr-element.sr-left', {origin: 'left'})
sr.reveal('.sr-element.sr-right', {origin: 'right'})
sr.reveal('.sr-element.sr-top', {origin: 'top'})
sr.reveal('.sr-element.sr-bottom', {origin: 'bottom'})
sr.reveal('.sr-element.sr-offset-50', {viewOffset: { bottom: 50}})
sr.reveal('.sr-element.sr-offset-100', {viewOffset: { bottom: 100}})
sr.reveal('.sr-element.sr-offset-150', {viewOffset: { bottom: 150}})
sr.reveal('.sr-element.sr-offset-200', {viewOffset: { bottom: 200}})
sr.reveal('.sr-element.sr-offset-250', {viewOffset: { bottom: 250}})
sr.reveal('.sr-element.sr-offset-300', {viewOffset: { bottom: 300}})
sr.reveal('.sr-element.sr-delay-200', {delay: 200})
sr.reveal('.sr-element.sr-delay-400', {delay: 400})
sr.reveal('.sr-element.sr-delay-600', {delay: 600})
sr.reveal('.sr-element.sr-delay-800', {delay: 800})
sr.reveal('.sr-element.sr-delay-1000', {delay: 1000})