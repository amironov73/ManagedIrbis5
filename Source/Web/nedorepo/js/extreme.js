const prefixes = [
    {prefix: 'A=', text: 'Автор'},
    {prefix: 'T=', text: 'Заглавие'},
    {prefix: 'MR=', text: 'Кафедра'},
]

let theData = {
    prefix1: '',
    input1: 'Начальное значение',
    check1: true,
    input2: 'Тоже начальное',
    value2: '',
    check2: true,
    input3: 'И здесь',
    value3: '',
    check3: false
}

$(function () {

    const form = $('#form').dxForm({
        formData: theData,
        items: [
            {
                itemType: 'group',
                colCount: 3,
                items: [{
                    label: {visible: false},
                    dataField: 'prefix1',
                    editorType: 'dxSelectBox',
                    editorOptions: {
                        valueExpr: 'prefix',
                        displayExpr: 'text',
                        value: prefixes[0].prefix,
                        dataSource: prefixes
                    }
                }, {
                    label: {visible: false},
                    dataField: 'input1'
                }, {
                    label: {visible: false},
                    dataField: 'check1',
                }]
            },

            {
                itemType: 'group',
                colCount: 3,
                items: [{
                    label: {visible: false},
                    dataField: 'prefix2',
                    editorType: 'dxSelectBox',
                    editorOptions: {
                        valueExpr: 'prefix',
                        displayExpr: 'text',
                        value: prefixes[1].prefix,
                        dataSource: prefixes
                    }
                }, {
                    label: {visible: false},
                    dataField: 'input2'
                }, {
                    label: {visible: false},
                    dataField: 'check2',
                }]
            },

            {
                itemType: 'group',
                colCount: 3,
                items: [{
                    label: {visible: false},
                    dataField: 'prefix3',
                    editorType: 'dxSelectBox',
                    editorOptions: {
                        valueExpr: 'prefix',
                        displayExpr: 'text',
                        value: prefixes[2].prefix,
                        dataSource: prefixes
                    }
                }, {
                    label: {visible: false},
                    dataField: 'input3'
                }, {
                    label: {visible: false},
                    dataField: 'check3',
                }]
            }

        ]
    }).dxForm('instance')

    const button = $('#show-button').dxButton({
        type: 'success',
        text: 'Нажми меня',
        onClick: function (e) {
            let r = form.option('formData')
            //alert (r)
            result.option('value', JSON.stringify(r))
        }
    })

    const result = $('#result').dxTextBox({
        value: 'Пока ничего не вычислено'
    }).dxTextBox('instance')

});
