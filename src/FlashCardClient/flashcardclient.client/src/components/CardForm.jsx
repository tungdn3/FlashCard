import PropTypes from "prop-types";
import {
  Form as RRDForm,
} from "react-router-dom";
import { useState } from "react";
import Form from "react-bootstrap/Form";
import Row from "react-bootstrap/Row";
import Button from "react-bootstrap/Button";
import Spinner from "react-bootstrap/Spinner";
import axios from "axios";

CardForm.propTypes = {
  cardToEdit: PropTypes.object,
};

export default function CardForm({ cardToEdit }) {
  const [word, setWord] = useState(cardToEdit?.word);
  const [meaning, setMeaning] = useState(cardToEdit?.meaning);
  const [example, setExample] = useState(cardToEdit?.example);
  const [isExampleGenerating, setIsExampleGenerating] = useState(false);
  
  async function handleGenerateExample() {
    setIsExampleGenerating(true);
    try {
      const response = await axios.post(`/v1/sentence-suggestions`, {
        word: word,
      });
      if (response.status === 200) {
        const example = response.data.join(". ");
        setExample(example);
      } else {
        // display toast
      }
    } finally {
      setIsExampleGenerating(false);
    }
  }

  return (
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

      <Form.Control
        hidden
        name="id"
        defaultValue={cardToEdit ? cardToEdit.id : ""}
      />

      <div className="d-flex">
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
  );
}
