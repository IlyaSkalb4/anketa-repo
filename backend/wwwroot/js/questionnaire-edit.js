// Global variables
let surveyId = null;
let saveTimeout;
const saveDelay = 1000; // 1 second delay

document.addEventListener('DOMContentLoaded', function () {
    // Get survey ID from URL or hidden field if available
    const urlParams = new URLSearchParams(window.location.search);
    surveyId = urlParams.get('id') || document.getElementById('surveyId')?.value;

    // Initialize event listeners
    initializeTitleAndDescriptionEvents();
    initializeQuestionTypeButtons();
    initializeDeleteFormButton();
});

function initializeTitleAndDescriptionEvents() {
    // Auto-save title
    const titleField = document.getElementById('surveyTitle');
    if (titleField) {
        titleField.addEventListener('input', function () {
            debounceSave(this);
        });
        
        titleField.addEventListener('blur', function () {
            saveImmediately(this);
        });
    }

    // Auto-save description
    const descriptionField = document.getElementById('surveyDescription');
    if (descriptionField) {
        descriptionField.addEventListener('input', function () {
            debounceSave(this);
        });
        
        descriptionField.addEventListener('blur', function () {
            saveImmediately(this);
        });
    }
}

function initializeQuestionTypeButtons() {
    // Add event listeners to question type buttons
    const questionTypeButtons = document.querySelectorAll('button[data-question-type]');
    questionTypeButtons.forEach(button => {
        button.addEventListener('click', function () {
            const inputType = this.getAttribute('data-question-type');
            addQuestion(inputType);
        });
    });
}

function initializeDeleteFormButton() {
    const deleteButton = document.querySelector('button[type="submit"].btn-danger');
    if (deleteButton) {
        deleteButton.addEventListener('click', function (e) {
            e.preventDefault();
            if (confirm('Ви впевнені, що хочете видалити цю анкету?')) {
                deleteQuestionnaire();
            }
        });
    }
}

// Debounce save function
function debounceSave(field) {
    showSavingIndicator();
    clearTimeout(saveTimeout);
    saveTimeout = setTimeout(() => {
        saveField(field);
    }, saveDelay);
}

// Save immediately on blur
function saveImmediately(field) {
    clearTimeout(saveTimeout);
    saveField(field);
}

// Show saving indicator
function showSavingIndicator() {
    // Create or update a saving indicator
    let indicator = document.getElementById('saving-indicator');
    if (!indicator) {
        indicator = document.createElement('div');
        indicator.id = 'saving-indicator';
        indicator.className = 'saving-indicator';
        indicator.textContent = 'Збереження...';
        document.querySelector('.container').appendChild(indicator);
    }
    indicator.style.display = 'block';
}

// Hide saving indicator
function hideSavingIndicator() {
    const indicator = document.getElementById('saving-indicator');
    if (indicator) {
        indicator.style.display = 'none';
    }
}

// Main save function
async function saveField(field) {
    try {
        // If survey doesn't exist yet, create it first
        if (!surveyId && (field.id === 'surveyTitle' || field.id === 'surveyDescription')) {
            await createSurvey();
            return;
        }

        // Update existing survey
        if (surveyId) {
            const title = document.getElementById('surveyTitle').value;
            const description = document.getElementById('surveyDescription').value;
            
            const response = await fetch('/Questionnaire/Update', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': getAntiForgeryToken()
                },
                body: JSON.stringify({
                    id: surveyId,
                    title: title,
                    description: description
                })
            });

            if (!response.ok) {
                showErrorMessage('Не вдалося оновити анкету');
            }
        }
    } catch (error) {
        console.error('Error saving data:', error);
        showErrorMessage('Сталася помилка під час збереження');
    } finally {
        hideSavingIndicator();
    }
}

// Create a new survey
async function createSurvey() {
    const title = document.getElementById('surveyTitle').value;
    const description = document.getElementById('surveyDescription').value;
    
    if (!title || !description) {
        showErrorMessage('Будь ласка, заповніть назву та опис');
        return;
    }

    try {
        showSavingIndicator();
        const response = await fetch('/Questionnaire/Create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken()
            },
            body: JSON.stringify({
                title: title,
                description: description
            })
        });

        if (response.ok) {
            const data = await response.json();
            surveyId = data.id;
            
            // Update URL without reloading the page
            const newUrl = `${window.location.pathname}?id=${surveyId}`;
            window.history.pushState({ id: surveyId }, '', newUrl);
            
            // Add hidden field for survey ID
            let hiddenField = document.getElementById('surveyId');
            if (!hiddenField) {
                hiddenField = document.createElement('input');
                hiddenField.type = 'hidden';
                hiddenField.id = 'surveyId';
                hiddenField.name = 'surveyId';
                document.querySelector('form').appendChild(hiddenField);
            }
            hiddenField.value = surveyId;
            
            showSuccessMessage('Анкета створена успішно');
        } else {
            showErrorMessage('Не вдалося створити анкету');
        }
    } catch (error) {
        console.error('Error creating survey:', error);
        showErrorMessage('Сталася помилка під час створення анкети');
    } finally {
        hideSavingIndicator();
    }
}

// Add a new question
async function addQuestion(inputType) {
    if (!surveyId) {
        showErrorMessage('Спочатку заповніть назву та опис');
        return;
    }

    try {
        const response = await fetch('/Questionnaire/AddQuestion', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken()
            },
            body: JSON.stringify({
                questionnaireId: surveyId,
                questionText: 'Нове питання',
                inputType: parseInt(inputType)
            })
        });

        if (response.ok) {
            const data = await response.json();
            
            // Create question container if it doesn't exist
            let questionContainer = document.getElementById('questionContainer');
            if (!questionContainer) {
                questionContainer = document.createElement('div');
                questionContainer.id = 'questionContainer';
                document.querySelector('.survey-content').appendChild(questionContainer);
            }
            
            // Insert the HTML
            questionContainer.insertAdjacentHTML('beforeend', data.html);
            
            // Initialize event listeners for the new question
            initializeQuestionEvents(data.id);
            
            showSuccessMessage('Питання додано успішно');
        } else {
            const errorData = await response.text();
            showErrorMessage(`Не вдалося додати питання: ${response.status} ${response.statusText}. ${errorData}`);
            console.error('Error response:', response.status, errorData);
        }
    } catch (error) {
        console.error('Error adding question:', error);
        showErrorMessage('Сталася помилка під час додавання питання: ' + error.message);
    }
}

// Initialize events for a question
function initializeQuestionEvents(questionId) {
    // Question text change event
    const questionInput = document.getElementById(`question${questionId}`);
    if (questionInput) {
        questionInput.addEventListener('blur', function() {
            updateQuestion(questionId, this.value);
        });
    }

    // Delete question button
    const deleteButton = document.querySelector(`[data-question-id="${questionId}"] .btn-trash`);
    if (deleteButton) {
        deleteButton.addEventListener('click', function() {
            deleteQuestion(questionId);
        });
    }

    // Add answer button
    const addAnswerButton = document.querySelector(`[data-question-id="${questionId}"] .btn-secondary`);
    if (addAnswerButton) {
        addAnswerButton.addEventListener('click', function() {
            addAnswerInput(questionId);
        });
    }
}

// Update a question
async function updateQuestion(questionId, text) {
    try {
        showSavingIndicator();
        const response = await fetch(`/Questionnaire/UpdateQuestion/${questionId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken()
            },
            body: JSON.stringify({
                questionText: text
            })
        });

        if (!response.ok) {
            showErrorMessage('Не вдалося оновити питання');
        }
    } catch (error) {
        console.error('Error updating question:', error);
        showErrorMessage('Сталася помилка під час оновлення питання');
    } finally {
        hideSavingIndicator();
    }
}

// Delete a question
async function deleteQuestion(questionId) {
    if (confirm('Ви впевнені, що хочете видалити це питання?')) {
        try {
            const response = await fetch(`/Questionnaire/DeleteQuestion/${questionId}`, {
                method: 'DELETE',
                headers: {
                    'RequestVerificationToken': getAntiForgeryToken()
                }
            });

            if (response.ok) {
                // Remove the question from the DOM
                const questionElement = document.querySelector(`[data-question-id="${questionId}"]`);
                if (questionElement) {
                    questionElement.remove();
                }
                showSuccessMessage('Питання видалено успішно');
            } else {
                showErrorMessage('Не вдалося видалити питання');
            }
        } catch (error) {
            console.error('Error deleting question:', error);
            showErrorMessage('Сталася помилка під час видалення питання');
        }
    }
}

// Add an answer input field
function addAnswerInput(questionId) {
    const answerContainer = document.querySelector(`[data-question-id="${questionId}"] .answer-container`);
    if (!answerContainer) return;

    const questionType = getQuestionType(questionId);
    let inputType = 'checkbox';
    
    if (questionType === 3) {
        inputType = 'radio';
    }

    const answerHtml = `
    <div class="mb-2 align-items-center d-flex">
        <input type="${inputType}" class="form-check-input me-2 fs-5" disabled>
        <input type="text" class="form-control w-50" placeholder="Відповідь" onblur="addAnswer(${questionId}, this.value)">
        <span class="rounded-circle close-button ms-2" onclick="removeAnswerInput(this)">
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16"
                fill="currentColor" class="bi bi-x-lg" viewBox="0 0 16 16">
                <path d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8z">
                </path>
            </svg>
        </span>
    </div>`;

    answerContainer.insertAdjacentHTML('beforeend', answerHtml);
}

// Helper to determine question type
function getQuestionType(questionId) {
    // This is a simplistic approach; you might want to store this information in a data attribute
    const questionElement = document.querySelector(`[data-question-id="${questionId}"]`);
    if (questionElement) {
        // First check if there's a data attribute with the input type
        const answerContainer = questionElement.querySelector('.answer-container');
        if (answerContainer && answerContainer.hasAttribute('data-input-type')) {
            return parseInt(answerContainer.getAttribute('data-input-type'));
        }
        
        // Otherwise try to determine from the HTML
        if (questionElement.querySelector('input[type="radio"]')) {
            return 3; // Radio = 3
        } else if (questionElement.querySelector('input[type="checkbox"]')) {
            return 4; // Checkbox = 4
        } else if (questionElement.querySelector('input[type="text"]:not(.form-control.fs-5)')) {
            return 1; // Text = 1
        } else if (questionElement.querySelector('input[type="number"]')) {
            return 2; // Number = 2
        } else if (questionElement.querySelector('input[type="date"]')) {
            return 5; // Date = 5
        } else if (questionElement.querySelector('input[type="time"]')) {
            return 6; // Time = 6
        }
    }
    
    // Default to text if we can't determine
    return 1;
}

// Remove an answer input field
function removeAnswerInput(element) {
    const answerDiv = element.closest('.mb-2');
    if (answerDiv) {
        // If this answer has an ID, delete it from the database
        const answerId = answerDiv.getAttribute('data-answer-id');
        if (answerId) {
            deleteAnswer(answerId);
        }
        answerDiv.remove();
    }
}

// Add an answer to a question
async function addAnswer(questionId, text) {
    if (!text) return null;

    try {
        showSavingIndicator();
        const response = await fetch('/Questionnaire/AddAnswer', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken()
            },
            body: JSON.stringify({
                questionId: questionId,
                textValue: text
            })
        });

        if (response.ok) {
            const data = await response.json();
            
            // Update the input field to include the answer ID (not doing UI changes here anymore)
            return data; // Return answer data to caller
        } else {
            showErrorMessage('Не вдалося додати відповідь');
            return null;
        }
    } catch (error) {
        console.error('Error adding answer:', error);
        showErrorMessage('Сталася помилка під час додавання відповіді');
        return null;
    } finally {
        hideSavingIndicator();
    }
}

// Update an answer
async function updateAnswer(answerId, text) {
    try {
        showSavingIndicator();
        const response = await fetch(`/Questionnaire/UpdateAnswer/${answerId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken()
            },
            body: JSON.stringify({
                textValue: text
            })
        });

        if (!response.ok) {
            showErrorMessage('Не вдалося оновити відповідь');
        }
    } catch (error) {
        console.error('Error updating answer:', error);
        showErrorMessage('Сталася помилка під час оновлення відповіді');
    } finally {
        hideSavingIndicator();
    }
}

// Delete an answer
async function deleteAnswer(answerId) {
    try {
        const response = await fetch(`/Questionnaire/DeleteAnswer/${answerId}`, {
            method: 'DELETE',
            headers: {
                'RequestVerificationToken': getAntiForgeryToken()
            }
        });

        if (!response.ok) {
            console.error('Не вдалося видалити відповідь');
        }
    } catch (error) {
        console.error('Error deleting answer:', error);
    }
}

// Delete the entire questionnaire
async function deleteQuestionnaire() {
    if (!surveyId) return;
    
    // Show the modal
    const modal = new bootstrap.Modal(document.getElementById('deleteModal'));
    modal.show();
}

// Confirm delete after modal confirmation
async function confirmDelete() {
    try {
        const response = await fetch(`/Questionnaire/Delete/${surveyId}`, {
            method: 'DELETE',
            headers: {
                'RequestVerificationToken': getAntiForgeryToken()
            }
        });

        if (response.ok) {
            // Redirect to the surveys list
            window.location.href = '/Surveys';
        } else {
            showErrorMessage('Не вдалося видалити анкету');
        }
    } catch (error) {
        console.error('Error deleting questionnaire:', error);
        showErrorMessage('Сталася помилка під час видалення анкети');
    }
}

// Duplicate a question
async function duplicateQuestion(questionId) {
    try {
        // Get the original question
        const questionElement = document.querySelector(`[data-question-id="${questionId}"]`);
        if (!questionElement) return;

        const questionText = document.getElementById(`question${questionId}`).value;
        const inputType = getQuestionType(questionId);
        const answers = Array.from(questionElement.querySelectorAll('.answer-container input[type="text"]:not(.form-control.fs-5)'))
            .map(input => input.value)
            .filter(value => value);

        // Create a new question with the same type
        const response = await fetch('/Questionnaire/AddQuestion', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken()
            },
            body: JSON.stringify({
                questionnaireId: surveyId,
                questionText: questionText,
                inputType: inputType
            })
        });

        if (response.ok) {
            const data = await response.json();
            
            // Create question container if it doesn't exist
            let questionContainer = document.getElementById('questionContainer');
            if (!questionContainer) {
                questionContainer = document.createElement('div');
                questionContainer.id = 'questionContainer';
                document.querySelector('.survey-content').appendChild(questionContainer);
            }
            
            // Insert the HTML
            questionContainer.insertAdjacentHTML('beforeend', data.html);
            
            // Initialize event listeners for the new question
            initializeQuestionEvents(data.id);

            // Add the answers and update the UI
            const newQuestionElement = document.querySelector(`[data-question-id="${data.id}"]`);
            const answerContainer = newQuestionElement.querySelector('.answer-container');
            
            // Clear any default answer input (if any)
            const defaultInputs = answerContainer.querySelectorAll('.mb-2');
            if (defaultInputs && defaultInputs.length > 0) {
                // Keep only the first one as a template and remove others
                for (let i = 1; i < defaultInputs.length; i++) {
                    defaultInputs[i].remove();
                }
            }
            
            // Only proceed with adding answers if this is a question type that supports answers
            if ([3, 4].includes(inputType)) { // Radio=3, Checkbox=4
                for (const answerText of answers) {
                    if (answerText.trim()) {
                        // Add to database
                        const answerResponse = await addAnswer(data.id, answerText);
                        
                        // Clone the template and update it with the answer text
                        if (answerResponse && answerContainer) {
                            const template = answerContainer.querySelector('.mb-2');
                            if (template) {
                                const newAnswer = template.cloneNode(true);
                                const textInput = newAnswer.querySelector('input[type="text"]');
                                if (textInput) {
                                    textInput.value = answerText;
                                    textInput.setAttribute('data-answer-id', answerResponse.id);
                                    newAnswer.setAttribute('data-answer-id', answerResponse.id);
                                    
                                    // Update the onblur to use updateAnswer instead of addAnswer
                                    textInput.onblur = function() {
                                        updateAnswer(answerResponse.id, this.value);
                                    };
                                    
                                    // Add after the template
                                    template.parentNode.appendChild(newAnswer);
                                }
                            }
                        }
                    }
                }
                
                // Remove the template if we added at least one answer
                if (answers.length > 0) {
                    const template = answerContainer.querySelector('.mb-2');
                    if (template) {
                        template.remove();
                    }
                }
            }
        } else {
            let errorText = '';
            try {
                const errorData = await response.json();
                errorText = JSON.stringify(errorData);
            } catch (e) {
                errorText = await response.text();
            }
            
            showErrorMessage(`Не вдалося дублювати питання: ${response.status} ${response.statusText}. ${errorText}`);
            console.error('Error response:', response.status, errorText);
        }
    } catch (error) {
        console.error('Error duplicating question:', error);
        showErrorMessage('Сталася помилка під час дублювання питання: ' + error.message);
    }
}

// Helper to get the anti-forgery token
function getAntiForgeryToken() {
    return document.querySelector('input[name="__RequestVerificationToken"]').value;
}

// Display success message
function showSuccessMessage(message) {
    showMessage(message, 'success');
}

// Display error message
function showErrorMessage(message) {
    showMessage(message, 'danger');
}

// Generic message display
function showMessage(message, type) {
    const messageDiv = document.createElement('div');
    messageDiv.className = `alert alert-${type} message-popup`;
    messageDiv.textContent = message;
    document.body.appendChild(messageDiv);
    
    // Remove after 3 seconds
    setTimeout(() => {
        messageDiv.remove();
    }, 3000);
}

// Additional CSS for messages and saving indicator
const style = document.createElement('style');
style.textContent = `
.message-popup {
    position: fixed;
    top: 20px;
    right: 20px;
    z-index: 9999;
    animation: fadeIn 0.3s, fadeOut 0.3s 2.7s;
}

.saving-indicator {
    position: fixed;
    bottom: 20px;
    right: 20px;
    background-color: #f8f9fa;
    padding: 10px 15px;
    border-radius: 4px;
    box-shadow: 0 2px 5px rgba(0,0,0,0.2);
    z-index: 9998;
}

@keyframes fadeIn {
    from { opacity: 0; }
    to { opacity: 1; }
}

@keyframes fadeOut {
    from { opacity: 1; }
    to { opacity: 0; }
}
`;
document.head.appendChild(style); 