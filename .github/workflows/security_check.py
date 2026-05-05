import os
import requests

GROQ_API_KEY = os.getenv("GROQ_API_KEY")
URL = "https://api.groq.com/openai/v1/chat/completions"

def check_security(file_path):
    with open(file_path, "r") as f:
        code = f.read()

    prompt = f"Act as a Cyber Security Expert. Scan this code for vulnerabilities (SQL Injection, XSS, Weak Auth, etc.). If you find any, provide the fixed code and a brief explanation. Code:\n\n{code}"
    
    headers = {
        "Authorization": f"Bearer {GROQ_API_KEY}",
        "Content-Type": "application/json"
    }
    
    data = {
        "model": "llama3-8b-8192",
        "messages": [{"role": "user", "content": prompt}]
    }

    response = requests.post(URL, headers=headers, json=data)
    print(f"--- Analysis for {file_path} ---")
    print(response.json()['choices'][0]['message']['content'])

# Yalnız .cs və .json fayllarını yoxla (Maarif layihəsi üçün)
for root, dirs, files in os.walk("."):
    for file in files:
        if file.endswith(".cs") or file.endswith("appsettings.json"):
            check_security(os.path.join(root, file))