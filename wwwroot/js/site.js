$(document).ready(function () {
    $('.show-data').click(function () {
        var mainHeading = $(this).data('mainheading');
        var mainSubHeading = $(this).data('mainsubheading');
        $('#modalScrollableTitle').text(mainHeading);

        var code = $(this).data('mainsubheading');
        //$('#modalBodyContent').text(mainSubHeading);
        $('#modalBodyContent').html('<code class="language-csharp">' + code + '</code>');

        Prism.highlightAll();
    });
});

$(document).ready(function () {
    $('#moduleDropdown').change(function () {
        var moduleId = $(this).val();
        if (moduleId) {
            $.ajax({
                url: '/TechArticle/GetTopics',
                type: 'GET',
                data: { moduleId: moduleId },
                dataType: 'json',
                success: function (data) {
                    $('#topicDropdown').empty();
                    $('#topicDropdown').append($('<option>').text('Select Topic').attr('value', ''));
                    $.each(data, function (index, topic) {
                        $('#topicDropdown').append($('<option>').text(topic.techTopicName).attr('value', topic.techTopicId));
                    });
                }
            });
        } else {
            $('#topicDropdown').empty();
            $('#topicDropdown').append($('<option>').text('Select Topic').attr('value', ''));
        }
    });

    // Load modules initially
    $.ajax({
        url: '/TechArticle/GetModules',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#moduleDropdown').empty();
            $('#moduleDropdown').append($('<option>').text('Select Module').attr('value', ''));
            $.each(data, function (index, module) {
                $('#moduleDropdown').append($('<option>').text(module.techModulesName).attr('value', module.techModulesId));
            });
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
});

$(document).ready(function () {
    $.ajax({
        url: '/TechArticlesData/GetArticles',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#articleDropdown').empty();
            $('#articleDropdown').append($('<option>').text('Select Article').attr('value', ''));
            // Populate the dropdown with fetched article data
            $.each(data, function (index, article) {
                $('#articleDropdown').append($('<option>').text(article.main_Heading).attr('value', article.techArticleId));
            })
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
});

$(document).ready(function () {
    $("#categoryFilter").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});

$(document).ready(function () {
    // Fetch module names and populate the dropdown
    $.ajax({
        url: '/TechArticle/GetModules',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#moduleId').empty();
            $('#moduleId').append($('<option>').text('Select Module').attr('value', ''));
            $.each(data, function (index, module) {
                $('#moduleId').append($('<option>').text(module.techModulesName).attr('value', module.techModulesId));
            });
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });

    // Handle module selection change event
    $('#moduleId').change(function () {
        var moduleId = $(this).val();
        if (moduleId) {
            window.location.href = '/TechTopic/GetTopics?techModulesId=' + moduleId;
        }
    });
});
