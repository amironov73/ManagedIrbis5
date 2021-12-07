let theData = {
    input1: 'Начальное значение',
    input2: 'Тоже начальное',
    input3: 'И здесь'
}

$(function () {

    const form = $('#form').dxForm ({
        formData: theData,
        items: [{
            dataField: 'input1'
        }, {
            dataField: 'input2'
        }, {
            dataField: 'input3'
        }

        ]
    }).dxForm('instance')

    const button = $('#show-button').dxButton ({
        type: 'success',
        text: 'Нажми меня',
        onClick: function (e) {
            let r = form.option('formData')
            alert (r)
            result.option('value', JSON.stringify(r))
        }
    })

    const result = $('#result').dxTextBox ({
        value: 'Пока ничего не вычислено'
    }).dxTextBox ('instance')

});
