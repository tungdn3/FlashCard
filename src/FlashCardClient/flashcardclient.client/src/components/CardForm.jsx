import PropTypes from "prop-types";
import { Form as RRDForm } from "react-router-dom";
import { useState } from "react";
import axios from "axios";
import Form from "react-bootstrap/Form";
import Row from "react-bootstrap/Row";
import Button from "react-bootstrap/Button";
import Spinner from "react-bootstrap/Spinner";
import Toast from "react-bootstrap/Toast";
import ToastContainer from "react-bootstrap/ToastContainer";
import Image from "react-bootstrap/Image";

CardForm.propTypes = {
  cardToEdit: PropTypes.object,
};

export default function CardForm({ cardToEdit }) {
  const [word, setWord] = useState(cardToEdit?.word || "");
  const [meaning, setMeaning] = useState(cardToEdit?.meaning || "");
  const [example, setExample] = useState(cardToEdit?.example || "");
  const [isExampleGenerating, setIsExampleGenerating] = useState(false);
  const [imageUrl, setImageUrl] = useState(cardToEdit?.imageUrl);
  const [isImageGenerating, setIsImageGenerating] = useState(false);
  const [showToast, setShowToast] = useState(false);

  async function handleGenerateExample() {
    setIsExampleGenerating(true);
    try {
      const response = await axios.post(`/v1/sentence-suggestions`, {
        word: word,
      });
      const example = response.data.join(". ");
      setExample(example);
    } catch {
      setShowToast(true);
    } finally {
      setIsExampleGenerating(false);
    }
  }

  async function handleGenerateImage() {
    setIsImageGenerating(true);
    try {
      const response = await axios.post(`/v1/image-generations`, {
        word: word,
      });
      setImageUrl(response.data);
    } catch {
      setShowToast(true);
    } finally {
      setIsImageGenerating(false);
    }
  }

  return (
    <>
      <RRDForm method="post">
        <Form.Group className="mb-3">
          <Form.Control
            required
            maxLength={100}
            name="word"
            value={word}
            onChange={(e) => setWord(e.target.value)}
            placeholder="Word"
            autoFocus
          />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Control
            required
            maxLength={500}
            name="meaning"
            value={meaning}
            onChange={(e) => setMeaning(e.target.value)}
            as="textarea"
            rows={2}
            placeholder="Meaning"
          />
        </Form.Group>

        <Form.Group as={Row} className="mb-3">
          <div className="d-flex">
            <Form.Control
              maxLength={500}
              name="example"
              value={example}
              onChange={(e) => setExample(e.target.value)}
              as="textarea"
              rows={2}
              placeholder="Example"
              className="flex-grow-1 me-2"
              disabled={isExampleGenerating || !word}
            />
            <Button
              variant="warning"
              disabled={isExampleGenerating || !word}
              onClick={handleGenerateExample}
            >
              {isExampleGenerating && (
                <Spinner
                  size="sm"
                  as="span"
                  animation="border"
                  role="status"
                  aria-hidden="true"
                />
              )}
              <span className="mx-2">AI Generate</span>
            </Button>
          </div>
        </Form.Group>

        <div className="d-flex flex-column" style={{ width: 200, heigh: 200 }}>
          <Form.Control name="imageUrl" value={imageUrl} readOnly hidden />
          <Image src={imageUrl || "/image-placeholder.png"} />
          <Button
            variant="warning"
            disabled={isImageGenerating || !word || imageUrl}
            onClick={handleGenerateImage}
          >
            {isImageGenerating && (
              <Spinner
                size="sm"
                as="span"
                animation="border"
                role="status"
                aria-hidden="true"
              />
            )}
            <span className="mx-2">AI Generate Image</span>
          </Button>
        </div>

        <Form.Control
          hidden
          name="id"
          defaultValue={cardToEdit ? cardToEdit.id : ""}
        />

        <div className="d-flex mt-3">
          <Button
            name="intent"
            value={cardToEdit ? "edit" : "create"}
            variant="primary"
            type="submit"
            className="ms-auto"
          >
            Save
          </Button>
        </div>
      </RRDForm>

      <ToastContainer
        className="p-3"
        position="bottom-center"
        style={{ zIndex: 1 }}
      >
        <Toast
          show={showToast}
          onClose={() => setShowToast(false)}
          delay={5000}
          autohide
          bg="danger"
          className="text-white"
        >
          <Toast.Header>
            <strong className="me-auto">AI Generate</strong>
          </Toast.Header>
          <Toast.Body>
            <div>Generatioin failed</div>
            <div>Please check the word or try again later</div>
          </Toast.Body>
        </Toast>
      </ToastContainer>
    </>
  );
}
