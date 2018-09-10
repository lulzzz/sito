(function () {

    // File upload
    function handleFileSelect(event) {
        if (window.File && window.FileList && window.FileReader) {

            var files = event.target.files;

            for (var i = 0; i < files.length; i++) {
                var file = files[i];

                // Only image uploads supported
                if (!file.type.match('image'))
                    continue;

                var reader = new FileReader();
                reader.addEventListener("load", function (event) {
                    var image = new Image();
                    image.alt = file.name;
                    image.onload = function (e) {
                        image.setAttribute("data-filename", file.name);
                        image.setAttribute("width", image.width);
                        image.setAttribute("height", image.height);
                        tinymce.activeEditor.execCommand('mceInsertContent', false, image.outerHTML);
                    };
                    image.src = this.result;

                });

                reader.readAsDataURL(file);
            }

            document.body.removeChild(event.target);
        }
        else {
            console.log("Your browser does not support File API");
        }
    }

    // remove empty strings
    function removeEmpty(item) {
        var trimmedItem = item.trim();
        if (trimmedItem.length > 0) {
            return trimmedItem;
        }
    }

    // edit form
    var edit = document.getElementById("edit");
    // Setup editor
    var editPost = document.getElementById("Content");

    if (edit && editPost) {

        if (typeof window.orientation !== "undefined" || navigator.userAgent.indexOf('IEMobile') !== -1) {
            tinymce.init({
                selector: '#Content',
                theme: 'modern',
                mobile: {
                    theme: 'mobile',
                    plugins: ['autosave', 'lists', 'autolink'],
                    toolbar: ['undo', 'bold', 'italic', 'styleselect']
                }
            });

        }

        // Delete post
        var deleteButton = edit.querySelector(".delete");
        if (deleteButton) {
            deleteButton.addEventListener("click", function (e) {
                if (!confirm("Are you sure you want to delete the post?")) {
                    e.preventDefault();
                }
            });
        }
    }
})();