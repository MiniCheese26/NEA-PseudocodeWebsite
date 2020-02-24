const submitButton = document.getElementById("submit");
const resetButton = document.getElementById("reset");
const outputArea = document.getElementById("output");
const inputArea = document.getElementById("input");

function resetSubmit() {
    submitButton.innerHTML = "Submit";
    submitButton.disabled = false;
}

function failureFunction(textarea) {
    textarea.value = "Failed to contact translation service";
    resetSubmit();
}

submitButton.addEventListener("click", () => {
    submitButton.disabled = true;
    submitButton.innerHTML = "Processing...";
    const xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            const response = JSON.parse(this.responseText);

            if (response) {
                if (response["failed"]) {
                    outputArea.value = "Translation failed\n\n" + response["messages"].join("\n");
                } else {
                    outputArea.value = response["code"];
                }
            } else {
                outputArea.value = "Translation failed fatally";
            }

            resetSubmit(submitButton);
        }
    };

    const code = inputArea.value;

    if (!code || code.trim().length === 0) {
        outputArea.value = "Input area is empty!!!";
        resetSubmit(submitButton);
        return;
    }

    const versionSelect = document.getElementById("language-version");
    const versionSelected = versionSelect.options[versionSelect.selectedIndex].value;

    const url = "http://localhost:55712/home/translate?code=" + code + "&langaugeVersion=" + versionSelected;

    xhr.timeout = 5000;
    xhr.ontimeout = function () {
        failureFunction(outputArea);
    };
    xhr.onerror = function () {
        failureFunction(outputArea);
    };
    xhr.open("get", url, true);
    xhr.send();
});

resetButton.addEventListener("click", () => {
    inputArea.value = "";
    outputArea.value = "";
});


