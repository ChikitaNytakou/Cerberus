document.addEventListener("DOMContentLoaded", async function () {
    // Извлечение значения из атрибута данных
    var hasMultiplePolygonIds = document.body.getAttribute('data-has-multiple-polygon-ids');

    // Function to load polygons
    async function loadPolygons(selectElementId) {
        try {
            const response = await fetch('/SrVesGrPoezdaCoFour/GetPolygons');
            const polygons = await response.json();
            const polygonSelect = document.getElementById(selectElementId);
            polygonSelect.innerHTML = '';

            // Default option
            const defaultOption = document.createElement("option");
            defaultOption.value = "";
            defaultOption.text = "Выберите участок";
            defaultOption.disabled = true;
            defaultOption.selected = true;
            polygonSelect.appendChild(defaultOption);

            if (polygons && polygons.length > 0) {
                polygons.forEach(polygon => {
                    const option = document.createElement("option");
                    option.value = polygon.id;
                    option.text = polygon.name;
                    polygonSelect.appendChild(option);
                });
            } else {
                console.warn('Список полигонов пуст');
            }

            // Установка выбранного полигона
            const selectedPolygon = polygonSelect.dataset.selectedPolygon;
            if (selectedPolygon) {
                polygonSelect.value = selectedPolygon; // Установить значение для выбора
            }

        } catch (error) {
            console.error('Ошибка при загрузке полигонов:', error);
        }
    }
    loadPolygons('polygon');

    const polygonSelect = document.getElementById('polygon');
    const yearSelect = document.getElementById("yearSelect");
    const currentYear = new Date().getFullYear();
    const startYear = currentYear - 5;
    let isEditing = false; // Переменная для отслеживания режима редактирования

    // Добавление опций для выбора года
    for (let year = startYear; year <= currentYear; year++) {
        const option = document.createElement("option");
        option.value = year;
        option.text = year;
        yearSelect.appendChild(option);
    }

    // Установка выбранного года из ViewBag
    const selectedYear = yearSelect.dataset.selectedYear;
    yearSelect.value = selectedYear;

    // Функция для получения информации об изменениях
    function fetchInfoForCell(cell) {
        if (isEditing) return; // Если мы в режиме редактирования, выходим из функции
        let month = cell.dataset.month;
        let year = cell.dataset.year;
        const polygonId = polygonSelect.dataset.selectedPolygon;

        let absenceOfDelays = {
            Month: month,
            Year: year,
            Polygon: polygonId,
        };

        $.ajax({
            url: '/SrVesGrPoezdaCoFour/GetUpdateInfo',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(absenceOfDelays),
            success: function (response) {
                const lastUpdate = new Date(response.lastUpdated);
                const datePart = lastUpdate.toLocaleDateString();
                const timePart = lastUpdate.toLocaleTimeString();
                alert(`Дата изменения: ${datePart}\nВремя изменения: ${timePart}`);
            },
            error: function (xhr, status, error) {
                console.error("Ошибка:", xhr.responseText);
                alert('Ошибка!');
            }
        });
    }

    // Обработчики кликов на ячейки с данными
    const editableCells = document.querySelectorAll('.editable-cell');
    editableCells.forEach(cell => {
        cell.addEventListener('click', function () {
            fetchInfoForCell(this); // Используем this без jQuery
        });
    });

    const inputSelector = hasMultiplePolygonIds === 'true'
        ? '.plan-input, .factTkm-input-0, .factPkm-input-0, .factTkm-input-1, .factPkm-input-1'
        : '.plan-input, .factTkm-input-0, .factPkm-input-0';

    const inputs = document.querySelectorAll(inputSelector);
    inputs.forEach(input => {
        input.addEventListener('click', function (event) {
            event.stopPropagation();
        });
    });

    function setupButtonHandlers(index) {
        const editButtons = document.querySelectorAll(`.edit-btn-${index}`);
        editButtons.forEach(button => {
            button.addEventListener('click', function () {
                toggleEditMode(this, index);
            });
        });

        const saveButtons = document.querySelectorAll(`.save-btn-${index}`);
        saveButtons.forEach(button => {
            button.addEventListener('click', function () {
                handleSaveButtonClick(this, index);
            });
        });

        const cancelButtons = document.querySelectorAll(`.cancel-btn-${index}`);
        cancelButtons.forEach(button => {
            button.addEventListener('click', function () {
                toggleEditMode(this, index, false);
            });
        });
    }

    function toggleEditMode(button, index, isEditing = true) {
        const month = button.id.split('-')[1];
        const row = button.closest('tr');
        const planInput = row.querySelector('.plan-input');
        const factTkmInput = row.querySelector(`.factTkm-input-${index}`);
        const factPkmInput = row.querySelector(`.factPkm-input-${index}`);
        const planValue = row.querySelector('.plan-value');
        const factTkmValue = row.querySelector(`.factTkm-value-${index}`);
        const factPkmValue = row.querySelector(`.factPkm-value-${index}`);

        if (isEditing) {
            // Включаем режим редактирования
            planValue.style.display = 'none';
            factTkmValue.style.display = 'none';
            factPkmValue.style.display = 'none';
            planInput.style.display = 'inline';
            factTkmInput.style.display = 'inline';
            factPkmInput.style.display = 'inline';

            // Заполняем поля ввода значениями
            planInput.value = planValue.innerText;
            factTkmInput.value = factTkmValue.innerText;
            factPkmInput.value = factPkmValue.innerText;

            row.querySelector(`#editButton-${month}-${index}`).style.display = 'none';
            row.querySelector(`#saveButton-${month}-${index}`).style.display = 'inline-block';
            row.querySelector(`#cancelButton-${month}-${index}`).style.display = 'inline-block';
        } else {
            // Выключаем режим редактирования
            planInput.style.display = 'none';
            factTkmInput.style.display = 'none';
            factPkmInput.style.display = 'none';

            row.querySelector(`#editButton-${month}-${index}`).style.display = 'inline-block';
            row.querySelector(`#saveButton-${month}-${index}`).style.display = 'none';
            row.querySelector(`#cancelButton-${month}-${index}`).style.display = 'none';

            planValue.style.display = 'inline';
            factTkmValue.style.display = 'inline';
            factPkmValue.style.display = 'inline';
        }
    }

    function handleSaveButtonClick(button, index) {
        const month = button.id.split('-')[1];
        const row = button.closest('tr');
        const planInput = row.querySelector('.plan-input');
        const factTkmInput = row.querySelector(`.factTkm-input-${index}`);
        const factPkmInput = row.querySelector(`.factPkm-input-${index}`);
        const polygonInput = row.querySelector('.polygon-input');

        // Создаем массивы для FactTkm и FactPkm
        let factTkmArray = [];
        let factPkmArray = [];
        let idArray = []; // Массив для идентификаторов

        // Находим все строки таблицы
        const rows = document.querySelectorAll('tr');

        rows.forEach((currentRow) => {
            // Проверяем, есть ли кнопка с id, содержащим editButton и текущий месяц
            const editButton = currentRow.querySelector(`[id^="editButton-${month}-"]`);
            if (editButton) { // Если кнопка найдена
                const FactTkm1 = parseFloat(currentRow.querySelector(`.factTkm-input-${0}`).value);
                const FactPkm1 = parseFloat(currentRow.querySelector(`.factPkm-input-${0}`).value);

                if (hasMultiplePolygonIds === 'true') {
                    const FactTkm2 = parseFloat(currentRow.querySelector(`.factTkm-input-${1}`).value);
                    const FactPkm2 = parseFloat(currentRow.querySelector(`.factPkm-input-${1}`).value);
                    if (!isNaN(FactTkm2)) {
                        factTkmArray.push(FactTkm2);
                    }

                    if (!isNaN(FactPkm2)) {
                        factPkmArray.push(FactPkm2);
                    }
                }

                if (!isNaN(FactTkm1)) {
                    factTkmArray.push(FactTkm1);
                }

                if (!isNaN(FactPkm1)) {
                    factPkmArray.push(FactPkm1);
                }


                // Извлечение идентификаторов
                const idInputs = currentRow.querySelectorAll('.id-input');
                idInputs.forEach(input => {
                    idArray.push(input.value);
                });
            }
        });

        if (confirm('Вы уверены, что хотите изменить значение?')) {
            const plan = parseFloat(planInput.value);
            const factTkm = parseFloat(factTkmInput.value);
            const factPkm = parseFloat(factPkmInput.value);
            const polygonId = polygonInput.value; // Получаем значение полигона

            // Проверка на пустые и отрицательные значения
            if (validateInputs(plan, factTkm, factPkm)) {
                // AJAX-запрос для сохранения данных
                fetch(`/SrVesGrPoezdaCoFour/SaveSrVesGrPoezdaCoFour`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        Year: selectedYear,
                        Month: month,
                        Polygon: polygonId,
                        Plan: plan,
                        FactTkm: factTkmArray, // Передаем массив
                        FactPkm: factPkmArray, // Передаем массив
                        Id: idArray // Передаем массив идентификаторов
                    })
                })
                    .then(response => {
                        if (response.ok) {
                            updateValues(row, plan, factTkm, factPkm, index);
                            toggleEditMode(button, index, false);
                            window.location.reload(); // Обновление страницы
                        }
                    });
            }
        }
    }

    function validateInputs(plan, factTkm, factPkm) {
        if (isNaN(plan) || plan < 0) {
            alert('Пожалуйста, введите корректное значение для Плана (не меньше 0).');
            return false;
        }
        if (isNaN(factTkm) || factTkm < 0) {
            alert('Пожалуйста, введите корректное значение для Факта (не меньше 0).');
            return false;
        }
        if (isNaN(factPkm) || factPkm < 0) {
            alert('Пожалуйста, введите корректное значение для Факта (не меньше 0).');
            return false;
        }
        return true;
    }

    function updateValues(row, plan, factTkm, factPkm, index) {
        row.querySelector('.plan-value').innerText = plan;
        row.querySelector(`.factTkm-value-${index}`).innerText = factTkm;
        row.querySelector(`.factPkm-value-${index}`).innerText = factPkm;
        // Обновите значения факта и результата здесь, если они есть
    }

    // Инициализация обработчиков для обеих групп кнопок
    setupButtonHandlers(0);
    if (hasMultiplePolygonIds === 'true') {
        setupButtonHandlers(1);
    }
});