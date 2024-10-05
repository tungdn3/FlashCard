import {
  json,
  useLoaderData,
  useNavigation,
  useRevalidator,
} from "react-router-dom";
import { useEffect, useState } from "react";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import Spinner from "react-bootstrap/Spinner";
import axios from "axios";
import FlashCard from "../components/FlashCard";
import Learn from "../components/Learn";
import CardForm from "../components/CardForm";

export async function loader({ params }) {
  const response = await fetch(`/v1/decks/${params.deckId}/cards`);
  // todo: handle cookie expired properly
  if (response.status === 401) {
    window.location.href = "/auth/login";
  } else if (response.status === 500) {
    const error = await response.json();
    throw new Response("", {
      status: response.status,
      statusText: error.message,
    });
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
  const revalidator = useRevalidator();
  const navigation = useNavigation();
  const isLoading = navigation.state === "loading";

  const [showForm, setShowForm] = useState(false);
  const [showLearn, setShowLearn] = useState(false);
  const [cardToEdit, setCardToEdit] = useState(null);

  useEffect(() => {
    resetForm();
  }, [cards]);

  function handleCloseForm() {
    resetForm();
  }

  function resetForm() {
    setShowForm(false);
    setCardToEdit(null);
  }

  function handleEditCard(cardId) {
    const card = cards.find((x) => x.id === cardId);
    if (!card) {
      // todo: show to error page
      return;
    }
    setCardToEdit(card);
    setShowForm(true);
  }

  async function handleDeleteCard(cardId) {
    await axios.delete(`/v1/decks/${deckId}/cards/${cardId}`);
    revalidator.revalidate();
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

      {isLoading && (
        <div className="d-flex justify-content-center mt-5">
          <Spinner
            as="span"
            animation="border"
            role="status"
            aria-hidden="true"
          />
        </div>
      )}

      {!isLoading && !!cards && cards.length === 0 && (
        <div className="mt-5 text-center">
          <div className="text-secondary">
            Click the &quot;New Card&quot; button to add a card
          </div>
        </div>
      )}

      {!isLoading && !!cards && cards.length > 0 && (
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
      )}

      <Modal centered show={showForm} onHide={handleCloseForm}>
        <Modal.Header closeButton>
          <Modal.Title>{cardToEdit ? "Edit Card" : "Create Card"}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <CardForm cardToEdit={cardToEdit} />
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
