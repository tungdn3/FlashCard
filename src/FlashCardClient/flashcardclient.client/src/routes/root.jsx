import {
  Form as RForm,
  Outlet,
  useLoaderData,
  json,
  redirect,
  useSubmit,
} from "react-router-dom";
import { useEffect, useState } from "react";
import Header from "../components/Header";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Button from "react-bootstrap/Button";
import ListGroup from "react-bootstrap/ListGroup";
import Modal from "react-bootstrap/Modal";
import Form from "react-bootstrap/Form";
import axios from "axios";
import { useAuth } from "../context/AuthContext";
import DeckItem from "../components/DeckItem";

export async function loader({ request }) {
  const pageSize = 1000; // todo: handle paging properly
  const url = new URL(request.url);
  const searchText = url.searchParams.get("searchText");
  const pageNumber = parseInt(url.searchParams.get("page")) || 1;
  let apiUrl = `/v1/decks?pageSize=${pageSize}&pageNumber=${pageNumber}`;
  if (searchText) {
    apiUrl += `&searchText=${searchText}`;
  }
  const response = await fetch(apiUrl);
  if (response.status === 401) {
    return { decks: [], is401: true, searchText, pageNumber };
    // window.location.href = "/auth/login";
  }
  const pageResult = await response.json();
  return { decks: pageResult.items, searchText, pageNumber };
}

export async function action({ request }) {
  const formData = await request.formData();
  const intent = formData.get("intent");
  if (intent === "create") {
    const deckName = formData.get("name");
    const response = await axios.post(`/v1/decks`, { name: deckName });
    const id = response.data;
    return redirect(`/decks/${id}`);
  }

  if (intent === "delete") {
    const deckId = formData.get("id");
    await axios.delete(`/v1/decks/${deckId}`);
    return { ok: true };
  }

  throw json({ message: "Invalid intent" }, { status: 400 });
}

export default function Root() {
  const { decks, searchText, is401 } = useLoaderData();
  const { isAuthenticated, login } = useAuth();

  const isSessionExpired = is401 && isAuthenticated;
  if (isSessionExpired) {
    login();
  }

  const submit = useSubmit();
  const [query, setQuery] = useState(searchText);
  const [showCreateDeckModal, setShowCreateDeckModal] = useState(false);
  const [newDeckName, setNewDeckName] = useState("");

  // function handleSearchTextChanged(e) {
  //   setQuery(e.currentTarget.value);
  //   submit(e.currentTarget.form);
  // }

  useEffect(() => {
    setNewDeckName("");
  }, [decks]);

  return (
    <>
      <div className="d-flex flex-column container-fluid">
        <Header />
        <Container>
          <Row style={{ minHeight: "92vh" }}>
            <Col
              sm={3}
              className="p-3 pt-4 d-flex flex-column bg-body-secondary rounded rounded-top-0"
            >
              <div className="border-bottom pb-3">
                <div className="d-grid gap-2">
                  <Button
                    variant="secondary"
                    type="submit"
                    onClick={() => setShowCreateDeckModal(true)}
                    disabled={!isAuthenticated}
                  >
                    New Deck
                  </Button>
                </div>
                {/* <RForm className="mt-3">
                  <Form.Group className="mb-3">
                    <Form.Control
                      className="py-2"
                      placeholder="Type to search"
                      name="searchText"
                      value={query}
                      onChange={handleSearchTextChanged}
                      disabled={!isAuthenticated}
                    />
                  </Form.Group>
                </RForm> */}
              </div>

              <nav className="mt-2">
                {decks.length ? (
                  <ListGroup defaultActiveKey="3">
                    {decks.map((deck) => (
                      <DeckItem key={deck.id} id={deck.id} name={deck.name} />
                    ))}
                  </ListGroup>
                ) : (
                  <p>
                    <i>No decks</i>
                  </p>
                )}
              </nav>
            </Col>
            <Col sm={9}>
              <Outlet />
            </Col>
          </Row>
        </Container>
      </div>

      <Modal
        show={showCreateDeckModal}
        onHide={() => setShowCreateDeckModal(false)}
      >
        <Modal.Header closeButton>
          <Modal.Title>Create Deck</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <RForm method="post">
            <Form.Group className="mb-3">
              <Form.Control
                name="name"
                placeholder="Deck name"
                autoFocus
                onChange={(e) => setNewDeckName(e.target.value)}
              />
            </Form.Group>
            <div className="d-flex">
              <Button
                variant="primary"
                type="submit"
                name="intent"
                value="create"
                className="ms-auto"
                onClick={() => setShowCreateDeckModal(false)}
                disabled={!newDeckName}
              >
                Create
              </Button>
            </div>
          </RForm>
        </Modal.Body>
      </Modal>
    </>
  );
}
