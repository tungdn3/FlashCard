import { useLoaderData, NavLink } from "react-router-dom";
import Header from "../components/Header";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";

export async function loader({ params }) {
  const response = await fetch(`v1/decks/${params.deckId}`);
  const cards = await response.json();
  return { cards };
}

export default function Root() {
  const { cards } = useLoaderData();

  return (
    <>
      {cards.length ? (
        <ul>
          {cards.map((card) => (
            <NavLink key={card.id}>
              {card.word} - {card.meaning}
            </NavLink>
          ))}
        </ul>
      ) : (
        <p>
          <i>No cards</i>
        </p>
      )}
    </>
  );
}
