import {
  json,
  Form as RRDForm,
  useLoaderData,
  useRevalidator,
} from "react-router-dom";
import { useEffect, useState } from "react";
import Form from "react-bootstrap/Form";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import Spinner from "react-bootstrap/Spinner";
import axios from "axios";
import FlashCard from "../components/FlashCard";
import Learn from "../components/Learn";

export async function loader({ params }) {
  const response = await fetch(`/v1/decks/${params.deckId}/cards`);
  // todo: handle cookie expired properly
  if (response.status === 401) {
    window.location.href = "/auth/login";
  }
  const cards = await response.json();
  return { cards, deckId: params.deckId };
}

export async function action({ request, params }) {
  const formData = await request.formData();
  const card = Object.fromEntries(formData);
  const intent = formData.get("intent");

  if (intent === "create") {
    await axios.post(`/v1/decks/${params.deckId}/cards`, card);
    return { ok: true };
  }

  if (intent === "edit") {
    await axios.put(`/v1/decks/${params.deckId}/cards/${card.id}`, card);
    return { ok: true };
  }

  throw json({ message: `Invalid intent '${intent}'` }, { status: 400 });
}

export default function Deck() {
  const { cards, deckId } = useLoaderData();
  let revalidator = useRevalidator();

  const [showForm, setShowForm] = useState(false);
  const [showLearn, setShowLearn] = useState(false);
  const [cardToEdit, setCardToEdit] = useState(null);
  const [word, setWord] = useState("");
  const [example, setExample] = useState("");
  const [isExampleGenerating, setIsExampleGenerating] = useState(false);

  useEffect(() => {
    resetForm();
  }, [cards]);

  function handleSubmit() {
    setShowForm(false);
    // the logic of creating or editing is handled by the "action" function
  }

  function handleCloseForm() {
    setShowForm(false);
    resetForm();
  }

  function resetForm() {
    setCardToEdit(null);
    setWord("");
    setExample("");
  }

  function handleEditCard(cardId) {
    const card = cards.find((x) => x.id === cardId);
    if (!card) {
      // todo: show to error page
      return;
    }
    setCardToEdit(card);
    setWord(card.word);
    setExample(card.example);
    setShowForm(true);
  }

  async function handleDeleteCard(cardId) {
    await axios.delete(`/v1/decks/${deckId}/cards/${cardId}`);
    revalidator.revalidate();
  }

  async function handleGenerateExample() {
    setIsExampleGenerating(true);
    try {
      const response = await axios.post(`/v1/sentence-suggestions`, {
        word: word,
      });
      if (response.status === 200) {
        const example = response.data.join(". ");
        setExample(example);
        if (cardToEdit) {
          console.log("--------- set card to ediit example", example);
          setCardToEdit({ ...cardToEdit, example: example });
        }
      } else {
        // display toast
      }
    } finally {
      setIsExampleGenerating(false);
    }
  }

  return (
    <div className="mt-3">
      <div className="d-flex justify-content-end align-items-center">
        <Button variant="secondary" onClick={() => setShowForm(true)}>
          New Card
        </Button>
        <Button
          className="ms-3"
          disabled={!cards || !cards.length}
          onClick={() => setShowLearn(true)}
        >
          Learn
        </Button>
      </div>

      {cards && cards.length ? (
        <Row
          xs={1}
          md={2}
          lg={3}
          xxl={4}
          className="g-4"
          style={{ marginTop: "10rem", marginLeft: "1rem" }}
        >
          {cards.map((card) => (
            <Col key={card.id} style={{ minHeight: 300 }}>
              <FlashCard
                id={card.id}
                word={card.word}
                meaning={card.meaning}
                example={card.example}
                onEdit={handleEditCard}
                onDelete={handleDeleteCard}
              />
            </Col>
          ))}
        </Row>
      ) : (
        <div className="mt-5 text-center">
          <div className="text-secondary">
            Click the &quot;New Card&quot; button to add a card
          </div>
        </div>
      )}

      <Modal centered show={showForm} onHide={handleCloseForm}>
        <Modal.Header closeButton>
          <Modal.Title>{cardToEdit ? "Edit Card" : "Create Card"}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <RRDForm method="post" onSubmit={handleSubmit}>
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
                defaultValue={cardToEdit?.meaning}
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
        </Modal.Body>
      </Modal>

      <Modal show={showLearn} fullscreen onHide={() => setShowLearn(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Learn</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Learn
            cards={cards}
            onEdit={handleEditCard}
            onDelete={handleDeleteCard}
          />
        </Modal.Body>
      </Modal>
    </div>
  );
}
