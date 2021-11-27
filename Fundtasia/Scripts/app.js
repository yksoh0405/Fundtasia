// Escape regular expression
function escapeRegExp(string) {
    return string.replace(/[.*+\-?^${}()|[\]\\]/g, '\\$&');
}

// Initiate GET request to url provided
$('[data-get]').click(e => {
    e.preventDefault();
    let url = $(e.target).data('get');
    location = url || location;
});