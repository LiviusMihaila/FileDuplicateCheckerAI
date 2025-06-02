import requests

def extract_text_from_file(filepath: str) -> str:
    """
    Extracts plain text from a file using Apache Tika server.

    Args:
        filepath (str): Full path to the file to be processed.

    Returns:
        str: Extracted plain text.

    Raises:
        ValueError: If extracted text is too short to be meaningful.
        Exception: If Tika server returns an error.
    """
    with open(filepath, "rb") as f:
        headers = {"Accept": "text/plain"}
        response = requests.put("http://localhost:9998/tika", data=f, headers=headers)

    if response.status_code == 200:
        text = response.text.strip()
        if len(text) < 50:
            raise ValueError("Tika returned too little text — possible fallback required.")
        return text
    else:
        raise Exception(f"Tika error {response.status_code}: {response.text}")
