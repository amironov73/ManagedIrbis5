class SearchableList {

    constructor(name, title, prefix) {
        this.name = name
        this.title = title
        this.prefix = prefix
    }

    inputHandler(e) {
        const inputValue = e.target.value.toUpperCase()
        const that = e.target.root
        that.list.innerHTML = ''
        let filtered = []
        for (const item of that.data) {
            if (item.text.toUpperCase().includes(inputValue)) {
                filtered.push(item)
            }
        }

        that.populate(filtered)
    }

    render(rootElement) {
        const label = document.createElement('label')
        label.for = this.name
        const h3 = document.createElement('h3')
        h3.innerText = this.title
        label.appendChild(h3)
        rootElement.appendChild(label)

        this.input = document.createElement('input')
        this.input.id = name
        this.input.placeholder = 'Поиск по списку'
        this.input.type = 'text'
        this.input.className = 'form-control'
        rootElement.appendChild(this.input)

        this.list = document.createElement('ul')
        rootElement.appendChild(this.list)

        this.input.addEventListener('input', this.inputHandler)
        this.input.root = this
    }

    populate(selectedItems) {
        this.list.innerHTML = ''
        for (const item of selectedItems) {
            const li = document.createElement('li')
            const a = document.createElement('a')
            a.innerText = item.text
            a.href = this.prefix + item.expr
            a.target = '_blank'
            li.appendChild(a)
            this.list.appendChild(li)
        }
    }

    loadFrom(fileName) {
        axios.get(fileName)
            .then (response => {
                this.data = response.data
                this.populate(this.data)
            })
            .catch (function (error) {
                console.log(error)
                hideBusy ()
                showError ('Какая-то фигня')
            })

    }
}
