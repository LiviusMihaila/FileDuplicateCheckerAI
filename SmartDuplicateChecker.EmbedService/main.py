from fastapi import FastAPI, HTTPException, UploadFile, File
from pydantic import BaseModel
from sentence_transformers import SentenceTransformer
from tika_client import extract_text_from_file
import os
import re

# Initialize FastAPI app
app = FastAPI()

# Load embedding model (this will download it the first time)
model = SentenceTransformer("all-MiniLM-L6-v2")

# ----------------------------------------
# TEXT-BASED EMBEDDING ENDPOINT
# ----------------------------------------

class TextRequest(BaseModel):
    text: str

@app.post("/embed", summary="Generate embedding from raw text")
async def embed_text(req: TextRequest):
    """
    Receives raw text in JSON and returns the embedding vector.
    """
    if not req.text.strip():
        raise HTTPException(status_code=400, detail="Text is empty")

    embedding = model.encode(req.text).tolist()
    return {"embedding": embedding}

# ----------------------------------------
# FILE-BASED EMBEDDING ENDPOINT
# ----------------------------------------

@app.post("/file-embedding", summary="Generate embedding from uploaded file")
async def file_embedding(file: UploadFile = File(...)):
    """
    Receives a file, extracts text using Tika, and returns the embedding vector.
    """
    contents = await file.read()

    # Clean file name (only letters, digits, underscore, dash, dot)
    clean_filename = re.sub(r'[^a-zA-Z0-9_.-]', '_', file.filename)
    temp_path = f"temp_{clean_filename}"

    with open(temp_path, "wb") as f:
        f.write(contents)

    try:
        text = extract_text_from_file(temp_path)
        if not text.strip():
            raise HTTPException(status_code=400, detail="Empty text extracted from file")

        embedding = model.encode(text).tolist()
        return {"embedding": embedding}
    finally:
        if os.path.exists(temp_path):
            os.remove(temp_path)
